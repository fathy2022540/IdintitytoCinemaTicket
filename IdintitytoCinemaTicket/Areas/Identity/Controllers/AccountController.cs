using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using IdintitytoCinemaTicket.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static System.Net.WebRequestMethods;
using IdintitytoCinemaTicket.UnitOfWork;

namespace IdintitytoCinemaTicket.Areas.Identity.Controllers
{
    [Area("Identity")]
    public class AccountController : Controller
    {
        public AccountController(IUnitOfWork unitOfWork,
            IAccountService accountService,
            UserManager<ApplicationUser> userManager,
           SignInManager<ApplicationUser> signInManager,
           IEmailSender emailSender)
        {
            _UnitOfWork = unitOfWork;
            _AccountService = accountService;
            _UserManager = userManager;
            _SignInManager = signInManager;
            _EmailSender = emailSender;
        }
        public IUnitOfWork _UnitOfWork { get; }
        public IAccountService _AccountService { get; }
        public UserManager<ApplicationUser> _UserManager { get; }
        public SignInManager<ApplicationUser> _SignInManager { get; }
        public IEmailSender _EmailSender { get; }

        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm registerVm)
        {
            if (!ModelState.IsValid)
                return View(registerVm);

            var result = await _AccountService.RegisterAsync(registerVm, Request.Scheme);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                    ModelState.AddModelError(string.Empty, item.Description);

                return View(registerVm);
            }

            TempData["Success"] = "Please check your email to confirm your account.";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVm);
            }
            var result = await _AccountService.LoginAsync(loginVm);

            if (result.Succeeded)
            {
                TempData["Success"] = "Welcome Back!";
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("UsernameOrEmail",
                    "You tried too many attempts. Try again after 5 minutes.");
                TempData["Error"] = "You tried too many attempts. Try again after 5 minutes.";
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError("UsernameOrEmail", "Please confirm your email first!");
                TempData["Error"] = "Please confirm your email first!";
            }
            else
            {
                ModelState.AddModelError("Password", "Invalid Username Or Email Or Password");
                TempData["Error"] = "Invalid Username or Password!";
            }

            return View(loginVm);

        }
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string id)
        {
            var result = await _AccountService.ConfirmEmailAsync(id);

            if (!result.Succeeded)
            {
                TempData["Error"] = "Invalid or Expired Token!";
            }
            else
            {
                TempData["Success"] = "Email Confirmed Successfully!";
            }

            return RedirectToAction("Login", "Account", new { area = "Identity" });
        }


        [HttpGet]
        public IActionResult ResetConfirmationEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResendConfirmationEmail(string usernameOrEmail)
        {
            if (string.IsNullOrWhiteSpace(usernameOrEmail))
            {
                TempData["Error"] = "Please enter email or username";
                return RedirectToAction("Login");
            }

            var result = await _AccountService.SendConfirmationEmailAsync(usernameOrEmail, Request.Scheme);

            if (!result)
            {
                TempData["Error"] = "Failed to send confirmation email. Please check your input!";
                return RedirectToAction("Login");
            }

            TempData["Success"] = "Confirmation email has been sent, please check your inbox!";
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(FrogetPasswordVm vm, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _AccountService.ForgetPasswordAsync(vm.UserNameOrEmail, cancellationToken);

            if (!result)
            {
                TempData["Error"] = "Invalid email or too many attempts. Try again later!";
                return View(vm);
            }

            TempData["Success"] = "OTP has been sent to your email!";
            var user = await _UserManager.FindByNameAsync(vm.UserNameOrEmail)
                       ?? await _UserManager.FindByEmailAsync(vm.UserNameOrEmail);

            return RedirectToAction("ValidateOtp", new { userId = user!.Id });
        }
        [HttpGet]
        public IActionResult ValidateOtp(string UserId)
        {
            return View(new ValidateOTPVM
            {
                UserId = UserId
            });
        }
        [HttpPost]
        public async Task<IActionResult> ValidateOtp(ValidateOTPVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var isValidOtp = await _AccountService.ValidateOtpAsync(vm.UserId, vm.OTP);

            if (!isValidOtp)
            {
                TempData["Error"] = "Invalid or Expired OTP";
                return View(vm);
            }

            TempData["from-otp"] = Guid.NewGuid().ToString();
            TempData["Success"] = "OTP validated successfully!";

            return RedirectToAction("NewPassword", new { UserId = vm.UserId });
        }

        [HttpGet]
        public IActionResult NewPassword(string UserId)
        {
            if (TempData["from-otp"] is null)
                return NotFound();
            return View(new NewPasswordVM
            {
                UserId = UserId
            });
        }
        [HttpPost]
        public async Task<IActionResult> NewPassword(NewPasswordVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);


            var result = await _AccountService.ResetPasswordAsync(vm);

            if (!result)
            {
                TempData["Error"] = "Failed to reset password!";
                return View(vm);
            }

            TempData["Success"] = "Password reset successful!";
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            if (remoteError != null)
            {
                TempData["Error"] = $"Error from external provider: {remoteError}";
                return RedirectToAction("Login");
            }

            var result = await _AccountService.ExternalLoginCallbackAsync();

            if (result.Succeeded)
                return RedirectToAction("Index", "Home", new { area = "Admin" });

            TempData["Error"] = "External login failed!";
            return RedirectToAction("Login");
        }


    }
}
