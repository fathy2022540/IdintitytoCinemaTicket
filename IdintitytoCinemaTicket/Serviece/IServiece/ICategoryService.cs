using IdintitytoCinemaTicket.Models;

namespace IdintitytoCinemaTicket.Serviece.IServiece
{
    public interface ICategoryService
    {
        Task<string?> SaveMainImgAsync(Cinema cinema, IFormFile Img, CancellationToken cancellationToken);
        Task<string?> EditMainImgAsync(Cinema oldCinema, IFormFile Img, CancellationToken cancellationToken);
        string? Delete(Cinema cinema);
    }
}
