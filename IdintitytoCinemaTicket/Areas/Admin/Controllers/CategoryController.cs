using IdintitytoCinemaTicket.UnitOfWork;
using Microsoft.AspNetCore.Mvc;

namespace IdintitytoCinemaTicket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        public IUnitOfWork UnitOfWork { get; }


        public CategoryController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var allCategories = await UnitOfWork.CategoryReposatory.GetAsync(null, null, cancellationToken, false);
            return View(allCategories.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Category());
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category, CancellationToken cancellationToken)
        {

            await UnitOfWork.CategoryReposatory.CreateAsync(category, cancellationToken);
            await UnitOfWork.CategoryReposatory.CommitAsync();
            TempData["Success-Notification"] = "Category Created Succssefull";
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {

            var categoruy = await UnitOfWork.CategoryReposatory.GetOneAsync(c => c.Id == id, null, cancellationToken);

            if (categoruy == null)
                return NotFound();

            return View(categoruy);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {

            UnitOfWork.CategoryReposatory.Update(category);
            await UnitOfWork.CategoryReposatory.CommitAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {

            var category = await UnitOfWork.CategoryReposatory.GetOneAsync(c => c.Id == id, null, cancellationToken);

            if (category == null)
                return NotFound();

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {

            var category = await UnitOfWork.CategoryReposatory.GetOneAsync(c => c.Id == id, null, cancellationToken);
            if (category == null)
                return NotFound();


            UnitOfWork.CategoryReposatory.Remove(category);
            await UnitOfWork.CategoryReposatory.CommitAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
