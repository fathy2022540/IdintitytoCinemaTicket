using IdintitytoCinemaTicket.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace IdintitytoCinemaTicket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CinemaController : Controller
    {
        public IUnitOfWork UnitOfWork { get; }
        public ICategoryService CategoryService { get; }


        public CinemaController(IUnitOfWork unitOfWork, ICategoryService categoryService)
        {
            UnitOfWork = unitOfWork;
            CategoryService = categoryService;
        }
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var cinema = await UnitOfWork.CinemaReposatory.GetAsync(Tracked: false, cancellationToken: cancellationToken);

            return View(cinema.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Cinema cinema, IFormFile Img, CancellationToken cancellationToken)
        {

            await CategoryService.SaveMainImgAsync(cinema, Img, cancellationToken);

            await UnitOfWork.CinemaReposatory.CreateAsync(cinema, cancellationToken);
            await UnitOfWork.CinemaReposatory.CommitAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {

            var edit = await UnitOfWork.CinemaReposatory.GetOneAsync
                (expression: c => c.Id == id, null, cancellationToken, false);


            if (edit == null)
                return NotFound();

            return View(edit);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Cinema cinema, IFormFile? Img, CancellationToken cancellationToken)
        {

            var oldCinema = await UnitOfWork.CinemaReposatory.GetOneAsync(c => c.Id == cinema.Id, null, cancellationToken);

            if (oldCinema == null)
                return NotFound();


            oldCinema.Name = cinema.Name;
            oldCinema.Address = cinema.Address;
            oldCinema.Rate = cinema.Rate;
            oldCinema.Status = cinema.Status;
            if (Img is not null)
                await CategoryService.EditMainImgAsync(oldCinema, Img, cancellationToken);

            cinema.Img = oldCinema.Img;

            await UnitOfWork.CinemaReposatory.CommitAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {

            var cinema = await UnitOfWork.CinemaReposatory.GetOneAsync(c => c.Id == id, null, cancellationToken, false);
            if (cinema == null) return NotFound();
            return View(cinema);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {

            var cinema = await UnitOfWork.CinemaReposatory.GetOneAsync(c => c.Id == id, null, cancellationToken);
            if (cinema == null) return NotFound();
            CategoryService.Delete(cinema);

            UnitOfWork.CinemaReposatory.Remove(cinema);
            await UnitOfWork.CinemaReposatory.CommitAsync();
            return RedirectToAction("Index");
        }


    }
}
