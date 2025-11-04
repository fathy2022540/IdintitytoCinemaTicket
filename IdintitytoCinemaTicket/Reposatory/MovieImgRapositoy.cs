using IdintitytoCinemaTicket.Data;
using IdintitytoCinemaTicket.Reposatory.IRepositories;

namespace IdintitytoCinemaTicket.Reposatory
{
    public class MovieImgRapositoy : Repository<MoviesSupImg>, IMovieImgRapositoy
    {
        private ApplicationDbContext _context;
        public MovieImgRapositoy(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void RemoveRange(IEnumerable<MoviesSupImg> moviesImgs)
        {
            _context.RemoveRange(moviesImgs);
        }
    }
}
