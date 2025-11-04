using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdintitytoCinemaTicket.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        ApplicationDbContext _context = new ApplicationDbContext();
        public HomeController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; }

        public IActionResult Index()
        {
            var allCategory = _context.categories.AsNoTracking().AsEnumerable();
            var Cinemaa = _context.cinemas.AsNoTracking().AsEnumerable();
            var Movies = _context.movies.AsNoTracking().AsEnumerable();


            var currentUser = UserManager.GetUserAsync(User).Result;

            return View(new HomeVm()
            {
                movies = Movies.Count(),
                Cinemas = Cinemaa.Count(),
                Categories = allCategory.Count(),
                UserName = currentUser?.UserName
            });
        }

    }
}
