using IdintitytoCinemaTicket.Models;
using IdintitytoCinemaTicket.Reposatory.IRepositories;

namespace IdintitytoCinemaTicket.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {

        IRepository<Movie> MovieReposatory { get; }
        IRepository<Category> CategoryReposatory { get; }
        IRepository<Cinema> CinemaReposatory { get; }
        IActorImgRapositoy ActorImgRapositoy { get; }
        IMovieImgRapositoy SupImgRapositoy { get; }
        public IRepository<ApplicationUserOTP> ApplicationUserOTPrepository { get; }


        Task<int> CommitAsync();
    }
}
