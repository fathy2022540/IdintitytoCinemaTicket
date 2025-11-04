using System.ComponentModel.DataAnnotations;

namespace IdintitytoCinemaTicket.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Category Name")]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public List<Movie> Movies { get; set; }
    }
}
