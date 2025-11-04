using Microsoft.EntityFrameworkCore;

namespace IdintitytoCinemaTicket.Models
{
    [PrimaryKey("Id", "Img")]
    public class MoviesSupImg
    {

        public int Id { get; set; }
        public int MovieId { get; set; }
        public string Img { get; set; } = string.Empty;
        public Movie Movies { get; set; }
    }
}
