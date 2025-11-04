namespace IdintitytoCinemaTicket.Reposatory.IRepositories
{
    public interface IMovieImgRapositoy : IRepository<MoviesSupImg>
    {
        public void RemoveRange(IEnumerable<MoviesSupImg> moviesImgs);
    }
}
