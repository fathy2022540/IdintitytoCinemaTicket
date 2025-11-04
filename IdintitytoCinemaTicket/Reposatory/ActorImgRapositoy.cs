using IdintitytoCinemaTicket.Data;
using IdintitytoCinemaTicket.Reposatory.IRepositories;

namespace IdintitytoCinemaTicket.Reposatory
{
    public class ActorImgRapositoy : Repository<Actor>, IActorImgRapositoy
    {
        ApplicationDbContext _context;
        public ActorImgRapositoy(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void RemoveRange(IEnumerable<Actor> actors)
        {
            _context.RemoveRange(actors);
        }
    }
}
