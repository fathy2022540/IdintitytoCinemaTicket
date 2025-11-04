using IdintitytoCinemaTicket.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace IdintitytoCinemaTicket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        public IUnitOfWork UnitOfWork { get; }
        public IMovieService MovieService { get; }
        public MovieController(IUnitOfWork unitOfWork, IMovieService movieService)
        {
            UnitOfWork = unitOfWork;
            MovieService = movieService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {

            var movies = await UnitOfWork.MovieReposatory.GetAsync(null,
                [indexer => indexer.Category, i => i.Cinema],
                cancellationToken,
                false);

            return View(movies);
        }
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var moviVmmm = new MoviesVm()
            {

                categories = await UnitOfWork.CategoryReposatory.GetAsync(null,
                null,
                cancellationToken,
          false),
                cinemas = await UnitOfWork.CinemaReposatory.GetAsync(null,
                null,
                cancellationToken,
          false)
            };
            return View(moviVmmm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(MoviesVm moviesVm, IFormFile MainImg, List<IFormFile> SupImg, List<IFormFile> Actors, CancellationToken cancellationToken)
        {
            //main Img

            var minIimg = await MovieService.SaveMainImageAsync(MainImg, cancellationToken);
            if (minIimg is not null)
                moviesVm.Movie.MainImg = minIimg;

            var movieCreated = await UnitOfWork.MovieReposatory.CreateAsync(moviesVm.Movie, cancellationToken);
            await UnitOfWork.MovieReposatory.CommitAsync();

            //supImg
            await MovieService.SaveSupImgAsync(SupImg, cancellationToken, movieCreated);
            //Actor
            await MovieService.SaveActorImgAsync(Actors, cancellationToken, movieCreated);

            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {

            var movie = await UnitOfWork.MovieReposatory.GetOneAsync(c => c.Id == id, null, cancellationToken);
            if (movie == null)
                return NotFound();


            return View(new MoviesVm()
            {
                Movie = movie,
                categories = await UnitOfWork.CategoryReposatory.GetAsync(null, null, cancellationToken, false),
                cinemas = await UnitOfWork.CinemaReposatory.GetAsync(null, null, cancellationToken, false),
                Actors = await UnitOfWork.ActorImgRapositoy.GetAsync(a => a.MovieId == id, null, cancellationToken),
                SupImgs = await UnitOfWork.SupImgRapositoy.GetAsync(a => a.MovieId == id, null, cancellationToken)

            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(MoviesVm moviesVm, IFormFile? MainImg, List<IFormFile>? SupImg, List<IFormFile>? Actors, CancellationToken cancellationToken)
        {


            var EditMovie = await UnitOfWork.MovieReposatory.GetOneAsync(c => c.Id == moviesVm.Movie.Id, null, cancellationToken, false);
            if (EditMovie == null)
                return NotFound();
            //min Img
            await MovieService.EditMainImgAsync(moviesVm, MainImg, cancellationToken, EditMovie);
            //SupImg
            await MovieService.EditSupImgAsync(moviesVm, SupImg, cancellationToken);
            //Actor
            await MovieService.EditActorAsync(moviesVm, Actors, cancellationToken);

            UnitOfWork.MovieReposatory.Update(moviesVm.Movie);
            await UnitOfWork.CommitAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {

            var movie = await UnitOfWork.MovieReposatory.GetOneAsync(null, [m => m.Category, m => m.Cinema], cancellationToken);
            if (movie == null)
                return NotFound();

            return View(movie);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {

            var movie = await UnitOfWork.MovieReposatory.GetOneAsync(c => c.Id == id,
                [m => m.Category, m => m.Cinema],
                cancellationToken);

            if (movie == null)
                return NotFound();


            MovieService.DeleteMainImg(movie);

            MovieService.DeleteSupImg(movie);

            MovieService.DeleteActor(movie);


            UnitOfWork.MovieReposatory.Remove(movie);
            await UnitOfWork.CommitAsync();

            return RedirectToAction("Index");
        }


    }
}
