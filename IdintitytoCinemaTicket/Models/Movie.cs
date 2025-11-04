using System.ComponentModel.DataAnnotations;

namespace IdintitytoCinemaTicket.Models
{
    public class Movie
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter Movie Name")]
        public string Name { get; set; } = string.Empty;
        public string? Title { get; set; }
        [Required]
        public decimal Price { get; set; }
        public bool Status { get; set; }

        public DateTime DateTime { get; set; }

        public string MainImg { get; set; } = string.Empty;

        public List<MoviesSupImg> SupImgs { get; set; }
        public List<Actor> Actors { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }

        public int CinemaId { get; set; }

        public Cinema Cinema { get; set; }
    }
}
