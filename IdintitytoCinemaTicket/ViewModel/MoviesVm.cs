namespace IdintitytoCinemaTicket.ViewModel
{
    public class MoviesVm
    {
        public Movie Movie { get; set; }
        public IEnumerable<Category> categories { get; set; }
        public IEnumerable<Cinema> cinemas { get; set; }
        public IEnumerable<Actor>? Actors { get; set; }
        public IEnumerable<MoviesSupImg>? SupImgs { get; set; }

    }
}
