namespace IdintitytoCinemaTicket.Reposatory.IRepositories
{
    public interface IActorImgRapositoy : IRepository<Actor>
    {
        public void RemoveRange(IEnumerable<Actor> actors);
    }
}
