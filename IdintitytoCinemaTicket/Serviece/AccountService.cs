using IdintitytoCinemaTicket.Models;
using IdintitytoCinemaTicket.Serviece.IServiece;
using IdintitytoCinemaTicket.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;

namespace IdintitytoCinemaTicket.Serviece
{
    public class AccountService : IAccountService
    {
        public AccountService(IUnitOfWork unitOfWork,
           UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IEmailSender emailSender
         )
        {
            _UnitOfWork = unitOfWork;
            _UserManager = userManager;
            _SignInManager = signInManager;
            _EmailSender = emailSender;

        }
        public IUnitOfWork _UnitOfWork { get; }
        public UserManager<ApplicationUser> _UserManager { get; }
        public SignInManager<ApplicationUser> _SignInManager { get; }
        public IEmailSender _EmailSender { get; }


        public async Task<IdentityResult> RegisterAsync(RegisterVm registerVm, string scheme)
        {

            var existingUser = await _UserManager.FindByEmailAsync(registerVm.Email);
            if (existingUser != null)
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Email is already in use"
                });

            var user = new ApplicationUser
            {
                Name = registerVm.Name,
                Email = registerVm.Email,
                UserName = registerVm.UserName
            };


            var result = await _UserManager.CreateAsync(user, registerVm.Password);
            if (!result.Succeeded)
                return result;


            var token = await _UserManager.GenerateEmailConfirmationTokenAsync(user);


            var link = $"{scheme}://localhost:7180/Identity/Account/ConfirmEmail?token={token}&id={user.Id}";

            await _EmailSender.SendEmailAsync(
            registerVm.Email,
            "Movie Teckets - confirm Email",
                $"<h1>To confirm your email, please click <a href='{link}'>Here</a></h1>"
            );

            return result;
        }

        public async Task<Microsoft.AspNetCore.Identity.SignInResult> LoginAsync(LoginVm model)
        {

            var user = await _UserManager.FindByNameAsync(model.UsernameOrEmail)
                        ?? await _UserManager.FindByEmailAsync(model.UsernameOrEmail);

            if (user is null)
                return Microsoft.AspNetCore.Identity.SignInResult.Failed;


            var result = await _SignInManager.PasswordSignInAsync(
                                user,
                                model.Password,
                                model.RememberMe,
                                lockoutOnFailure: true);

            return result;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string userId)
        {
            var user = await _UserManager.FindByIdAsync(userId);
            if (user is null)
                return IdentityResult.Failed(new IdentityError { Description = "User not found!" });

            var token = await _UserManager.GenerateEmailConfirmationTokenAsync(user);

            var result = await _UserManager.ConfirmEmailAsync(user, token);
            return result;
        }


        public async Task<bool> SendConfirmationEmailAsync(string usernameOrEmail, string scheme)
        {
            var user = await _UserManager.FindByNameAsync(usernameOrEmail)
                       ?? await _UserManager.FindByEmailAsync(usernameOrEmail);

            if (user is null)
                return false;

            if (user.EmailConfirmed)
                return false;

            var token = await _UserManager.GenerateEmailConfirmationTokenAsync(user);


            var link = $"{scheme}://localhost:7180/Identity/Account/ConfirmEmail?id={user.Id}&token={token}";

            await _EmailSender.SendEmailAsync(
                user.Email!,
                "Movie Tickets - Confirm Email",
                $"<h1>Click <a href='{link}'>Here</a> to confirm your email</h1>"
            );

            return true;
        }


        public async Task<bool> ForgetPasswordAsync(string usernameOrEmail, CancellationToken cancellationToken)
        {
            var user = await _UserManager.FindByNameAsync(usernameOrEmail)
                        ?? await _UserManager.FindByEmailAsync(usernameOrEmail);

            if (user is null)
                return false;

            var otp = new Random().Next(1000, 9999).ToString();

            var userOtp = await _UnitOfWork.ApplicationUserOTPrepository
                                .GetAsync(e => e.ApplicationUserId == user.Id);

            var totalOtp = userOtp.Count(e => (DateTime.UtcNow - e.CreateAt).TotalHours < 24);
            if (totalOtp > 3)
                return false;

            await _UnitOfWork.ApplicationUserOTPrepository.CreateAsync(new ApplicationUserOTP
            {
                ApplicationUserId = user.Id,
                CreateAt = DateTime.UtcNow,
                IsValid = true,
                OTP = otp,
                ValidTo = DateTime.UtcNow.AddMinutes(30)
            }, cancellationToken);

            await _UnitOfWork.ApplicationUserOTPrepository.CommitAsync();

            await _EmailSender.SendEmailAsync(
                user.Email!,
                "Movie Teckets - Forget Password",
                $"<h1>Use this OTP to reset your password: <strong>{otp}</strong><br/>Do not share it!</h1>"
            );

            return true;
        }


        public async Task<bool> ValidateOtpAsync(string userId, string otp)
        {
            var userOtpList = await _UnitOfWork.ApplicationUserOTPrepository
                                .GetAsync(e => e.ApplicationUserId == userId
                                            && e.IsValid
                                            && e.ValidTo > DateTime.UtcNow);

            if (userOtpList is null)
                return false;

            var matchedOtp = userOtpList.FirstOrDefault(e => e.OTP == otp);

            if (matchedOtp is null)
                return false;


            matchedOtp.IsValid = false;
            await _UnitOfWork.ApplicationUserOTPrepository.CommitAsync();

            return true;
        }


        public async Task<bool> ResetPasswordAsync(NewPasswordVM model)
        {
            var user = await _UserManager.FindByIdAsync(model.UserId);
            if (user is null)
                return false;

            var token = await _UserManager.GeneratePasswordResetTokenAsync(user);

            var result = await _UserManager.ResetPasswordAsync(user, token, model.NewPassword);
            var otps = await _UnitOfWork.ApplicationUserOTPrepository
            .GetAsync(e => e.ApplicationUserId == model.UserId && e.IsValid);

            foreach (var otp in otps)
                otp.IsValid = false;

            await _UnitOfWork.ApplicationUserOTPrepository.CommitAsync();

            return result.Succeeded;
        }


        public async Task<Microsoft.AspNetCore.Identity.SignInResult> ExternalLoginCallbackAsync()
        {
            var info = await _SignInManager.GetExternalLoginInfoAsync();
            if (info == null)
                return Microsoft.AspNetCore.Identity.SignInResult.Failed;

            // تسجيل دخول لو مسبقًا مسجل Login Provider
            var loginResult = await _SignInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false
            );

            if (loginResult.Succeeded)
                return loginResult;


            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var username = info.Principal.FindFirstValue(ClaimTypes.Name);

            if (email == null)
                return Microsoft.AspNetCore.Identity.SignInResult.Failed;

            var user = await _UserManager.FindByEmailAsync(email);

            if (user == null)
            {

                user = new ApplicationUser
                {
                    Email = email,
                    EmailConfirmed = true,
                    UserName = username!.Replace(" ", "") + new Random().Next(1000, 9999)
                };

                var createUserResult = await _UserManager.CreateAsync(user);
                if (!createUserResult.Succeeded)
                    return Microsoft.AspNetCore.Identity.SignInResult.Failed;
            }


            var addLoginResult = await _UserManager.AddLoginAsync(user, info);
            if (!addLoginResult.Succeeded)
                return Microsoft.AspNetCore.Identity.SignInResult.Failed;

            await _SignInManager.SignInAsync(user, isPersistent: false);

            return Microsoft.AspNetCore.Identity.SignInResult.Success;
        }

    }
}
