using IdintitytoCinemaTicket.Data;
using IdintitytoCinemaTicket.Models;
using IdintitytoCinemaTicket.UnitOfWork;
using IdintitytoCinemaTicket.Reposatory.IRepositories;
using Microsoft.EntityFrameworkCore;


namespace IdintitytoCinemaTicket.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(IRepository<Movie> movieReposatory,
            IRepository<Category> categoryReposatory,
            IRepository<Cinema> cinemaReposatory,
            IRepository<ApplicationUserOTP> ApplicationUserOTPrepository,
            IActorImgRapositoy actorImgRapositoy,
            IMovieImgRapositoy supImgRapositoy,
            ApplicationDbContext context

            )

        {
            MovieReposatory = movieReposatory;
            CategoryReposatory = categoryReposatory;
            CinemaReposatory = cinemaReposatory;
            this.ApplicationUserOTPrepository = ApplicationUserOTPrepository;
            ActorImgRapositoy = actorImgRapositoy;
            SupImgRapositoy = supImgRapositoy;
            _context = context;

        }

        public IRepository<Movie> MovieReposatory { get; set; }

        public IRepository<Category> CategoryReposatory { get; set; }

        public IRepository<Cinema> CinemaReposatory { get; set; }
        public IRepository<ApplicationUserOTP> ApplicationUserOTPrepository { get; }
        public IActorImgRapositoy ActorImgRapositoy { get; set; }

        public IMovieImgRapositoy SupImgRapositoy { get; set; }




        public void Dispose()
        {
            _context.Dispose();
        }
        public async Task<int> CommitAsync()
        {
            try
            {
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error : {ex.Message}");
                return 0;
            }

        }
    }
}
