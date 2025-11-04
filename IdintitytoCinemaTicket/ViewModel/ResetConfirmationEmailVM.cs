using System.ComponentModel.DataAnnotations;

namespace IdintitytoCinemaTicket.ViewModel
{
    public class ResetConfirmationEmailVM
    {
        [Required(ErrorMessage = "Enter Your Email Or UserName")]
        public string UserNameOrEmail { get; set; } = string.Empty;
    }
}
