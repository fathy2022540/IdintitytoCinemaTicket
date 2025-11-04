using System.ComponentModel.DataAnnotations;

namespace IdintitytoCinemaTicket.Models
{
    public class Cinema
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Cinema Name")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter Cinema Address")]
        public string Address { get; set; } = string.Empty;
        [Required]
        public bool Status { get; set; }
        public int? Rate { get; set; }
        public string Img { get; set; } = string.Empty;
        public List<Movie> Movies { get; set; }

    }
}
