using System.ComponentModel.DataAnnotations;

namespace IdintitytoCinemaTicket.ViewModel
{
    public class NewPasswordVM
    {
        [Required, DataType(DataType.Password)]
        public string NewPassword { get; set; } = string.Empty;
        [Compare("NewPassword", ErrorMessage = " not match Password"), DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;

    }
}
