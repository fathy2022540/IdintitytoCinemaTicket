using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace IdintitytoCinemaTicket.Serviece.IServiece
{
    public interface IAccountService
    {
        Task<IdentityResult> RegisterAsync(RegisterVm model, string scheme);
        Task<Microsoft.AspNetCore.Identity.SignInResult> LoginAsync(LoginVm model);
        Task<bool> SendConfirmationEmailAsync(string usernameOrEmail, string scheme);
        Task<bool> ForgetPasswordAsync(string usernameOrEmail, CancellationToken cancellationToken);
        Task<bool> ValidateOtpAsync(string userId, string otp);
        Task<bool> ResetPasswordAsync(NewPasswordVM model);
        Task<IdentityResult> ConfirmEmailAsync(string userId);

        Task<Microsoft.AspNetCore.Identity.SignInResult> ExternalLoginCallbackAsync();
    }
}
