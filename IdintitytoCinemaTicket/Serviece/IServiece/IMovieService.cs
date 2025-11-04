using IdintitytoCinemaTicket.Models;

namespace IdintitytoCinemaTicket.Serviece.IServiece
{
    public interface IMovieService
    {
        //save
        Task<string?> SaveMainImageAsync(IFormFile MainImg, CancellationToken cancellationToken);
        Task<string?> SaveSupImgAsync(List<IFormFile> SupImg, CancellationToken cancellationToken, Movie movieCreated);
        Task<string?> SaveActorImgAsync(List<IFormFile> SupImg, CancellationToken cancellationToken, Movie movieCreated);

        //edit
        Task<string?> EditMainImgAsync(MoviesVm moviesVm, IFormFile? MainImg, CancellationToken cancellationToken, Movie EditMovie);
        Task<string?> EditSupImgAsync(MoviesVm moviesVm, List<IFormFile>? SupImg, CancellationToken cancellationToken);
        Task<string?> EditActorAsync(MoviesVm moviesVm, List<IFormFile>? Actors, CancellationToken cancellationToken);

        //delete
        string? DeleteActor(Movie movie);
        string? DeleteSupImg(Movie movie);
        string? DeleteMainImg(Movie movie);
    }
}
