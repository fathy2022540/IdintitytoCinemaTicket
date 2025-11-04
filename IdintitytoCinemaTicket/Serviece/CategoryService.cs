using IdintitytoCinemaTicket.Models;
using IdintitytoCinemaTicket.Serviece.IServiece;
using IdintitytoCinemaTicket.UnitOfWork;

namespace IdintitytoCinemaTicket.Serviece
{
    public class CategoryService : ICategoryService
    {
        public CategoryService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork { get; }

        //save

        public async Task<string?> SaveMainImgAsync(Cinema cinema, IFormFile Img, CancellationToken cancellationToken)
        {
            if (Img is not null && Img.Length > 0)
            {
                var ImgName = Guid.NewGuid().ToString() + Path.GetExtension(Img.FileName);
                var ImgPath = Path.Combine(Directory.GetCurrentDirectory()
                    , "wwwroot/assets/img/cinemaImg", ImgName);
                using (var system = System.IO.File.Create(ImgPath))
                {
                    await Img.CopyToAsync(system);
                }
                cinema.Img = ImgName;
            }
            return null;
        }

        //edit
        public async Task<string?> EditMainImgAsync(Cinema oldCinema, IFormFile Img, CancellationToken cancellationToken)
        {
            if (Img != null && Img.Length > 0)
            {

                if (!string.IsNullOrEmpty(oldCinema.Img))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot/assets/img/cinemaImg", oldCinema.Img);
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);
                }


                var imgName = Guid.NewGuid() + Path.GetExtension(Img.FileName);
                var imgPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/assets/img/cinemaImg", imgName);

                using (var stream = System.IO.File.Create(imgPath))
                {
                    await Img.CopyToAsync(stream);
                }

                oldCinema.Img = imgName;
            }
            return "Img Edit";
        }

        //Delete
        public string? Delete(Cinema cinema)
        {
            if (!string.IsNullOrEmpty(cinema.Img))
            {
                var oldImgPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/assets/img/cinemaImg", cinema.Img);

                if (System.IO.File.Exists(oldImgPath))
                {
                    System.IO.File.Delete(oldImgPath);
                }
            }
            return "deleted sucssec";
        }

    }
}
