using Microsoft.AspNetCore.Identity;

namespace IdintitytoCinemaTicket.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public string? Address { get; set; }
    }
}
