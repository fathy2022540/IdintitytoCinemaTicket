using IdintitytoCinemaTicket.Models;
using IdintitytoCinemaTicket.Serviece.IServiece;
using IdintitytoCinemaTicket.UnitOfWork;

namespace IdintitytoCinemaTicket.Serviece
{
    public class MovieService : IMovieService
    {
        public MovieService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public IUnitOfWork UnitOfWork { get; }


        //Save
        public async Task<string?> SaveMainImageAsync(IFormFile MainImg, CancellationToken cancellationToken)
        {
            if (MainImg is not null && MainImg.Length > 0)
            {
                var MainImgName = Guid.NewGuid().ToString() + Path.GetExtension(MainImg.FileName);


                var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                var MainImgPath = Path.Combine(rootPath, "assets/img/MovieMainImg", MainImgName);


                using (var stream = new FileStream(MainImgPath, FileMode.Create))
                {
                    await MainImg.CopyToAsync(stream, cancellationToken);
                }


                return MainImgName;
            }


            return null;
        }

        public async Task<string?> SaveSupImgAsync(List<IFormFile> SupImg, CancellationToken cancellationToken, Movie movieCreated)
        {
            if (SupImg is not null && SupImg.Count() > 0)
            {
                foreach (var supImg in SupImg)
                {
                    var SupImgName = Guid.NewGuid().ToString()
                    + Path.GetExtension(supImg.FileName);
                    var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    var SupImgPath = Path.Combine(rootPath, "assets/img/MovieSupImg", SupImgName);

                    using (var system = System.IO.File.Create(SupImgPath))
                    {
                        await supImg.CopyToAsync(system);
                    }

                    await UnitOfWork.SupImgRapositoy.CreateAsync(new()
                    {
                        Img = SupImgName,
                        MovieId = movieCreated.Id
                    }, cancellationToken);

                }
                await UnitOfWork.CommitAsync();

            }
            return null;
        }

        public async Task<string?> SaveActorImgAsync(List<IFormFile> Actors, CancellationToken cancellationToken, Movie movieCreated)
        {
            if (Actors is not null && Actors.Count() > 0)
            {
                foreach (var actors in Actors)
                {
                    var actorsImgName = Guid.NewGuid().ToString()
                    + Path.GetExtension(actors.FileName);
                    var rootPath = Path.Combine(Directory.GetCurrentDirectory()
                        , "wwwroot");
                    var actorsImgPath = Path.Combine(rootPath, "assets/img/ActorsImg", actorsImgName);


                    using (var system = System.IO.File.Create(actorsImgPath))
                    {
                        await actors.CopyToAsync(system);
                    }
                    await UnitOfWork.ActorImgRapositoy.CreateAsync(new()
                    {
                        ActorImg = actorsImgName,
                        MovieId = movieCreated.Id
                    }, cancellationToken);
                }
                await UnitOfWork.CommitAsync();
            }
            return null;
        }
        //Edit
        public async Task<string?> EditMainImgAsync(MoviesVm moviesVm, IFormFile? MainImg, CancellationToken cancellationToken, Movie EditMovie)
        {
            if (MainImg is not null)
            {
                if (MainImg.Length > 0)
                {

                    if (!string.IsNullOrEmpty(EditMovie.MainImg))
                    {
                        var rootpath = Path.Combine(Directory.GetCurrentDirectory(),
                            "wwwroot");
                        var oldImgPath = Path.Combine(rootpath, "assets/img/MovieMainImg", EditMovie.MainImg);

                        if (System.IO.File.Exists(oldImgPath))
                        {
                            System.IO.File.Delete(oldImgPath);
                        }
                    }
                    var MainImgName = Guid.NewGuid().ToString()
                   + Path.GetExtension(MainImg.FileName);
                    var MainImgPath = Path.Combine(Directory.GetCurrentDirectory()
                        , "wwwroot/assets/img/MovieMainImg", MainImgName);

                    using (var system = System.IO.File.Create(MainImgPath))
                    {
                        await MainImg.CopyToAsync(system);
                    }
                    moviesVm.Movie.MainImg = MainImgName;
                    return MainImgName;

                }
            }
            else
            {
                moviesVm.Movie.MainImg = EditMovie.MainImg;
                return EditMovie.MainImg;
            }
            return "Main Img Edited";
        }

        public async Task<string?> EditSupImgAsync(MoviesVm moviesVm, List<IFormFile>? SupImg, CancellationToken cancellationToken)
        {
            if (SupImg is not null)
            {
                if (SupImg.Count > 0)
                {

                    var OldSupImg = await UnitOfWork.SupImgRapositoy.GetAsync(s => s.MovieId == moviesVm.Movie.Id, null, cancellationToken);
                    foreach (var item in OldSupImg)
                    {
                        if (!string.IsNullOrEmpty(item.Img))
                        {
                            var oldImgPath = Path.Combine(Directory.GetCurrentDirectory(),
                                "wwwroot/assets/img/MovieSupImg", item.Img);

                            if (System.IO.File.Exists(oldImgPath))
                            {
                                System.IO.File.Delete(oldImgPath);
                            }
                        }
                    }

                    UnitOfWork.SupImgRapositoy.RemoveRange(OldSupImg);
                    await UnitOfWork.CommitAsync();

                    foreach (var item in SupImg)
                    {

                        var SupImgName = Guid.NewGuid().ToString()
                             + Path.GetExtension(item.FileName);
                        var SupImgPath = Path.Combine(Directory.GetCurrentDirectory()
                            , "wwwroot/assets/img/MovieSupImg", SupImgName);

                        using (var system = System.IO.File.Create(SupImgPath))
                        {
                            await item.CopyToAsync(system);
                        }

                        await UnitOfWork.SupImgRapositoy.CreateAsync(new()
                        {
                            Img = SupImgName,
                            MovieId = moviesVm.Movie.Id
                        }, cancellationToken);
                    }
                    await UnitOfWork.CommitAsync();
                }
            }
            return "Sup Img Edited";
        }

        public async Task<string?> EditActorAsync(MoviesVm moviesVm, List<IFormFile>? Actors, CancellationToken cancellationToken)
        {
            if (Actors is not null)
            {
                if (Actors.Count > 0)
                {
                    var oldActors = await UnitOfWork.ActorImgRapositoy.GetAsync(a => a.MovieId == moviesVm.Movie.Id, null, cancellationToken);
                    foreach (var actor in oldActors)
                    {
                        if (!string.IsNullOrEmpty(actor.ActorImg))
                        {
                            var oldImgPath = Path.Combine(Directory.GetCurrentDirectory(),
                                "wwwroot/assets/img/ActorsImg", actor.ActorImg);

                            if (System.IO.File.Exists(oldImgPath))
                            {
                                System.IO.File.Delete(oldImgPath);
                            }
                        }
                    }
                    UnitOfWork.ActorImgRapositoy.RemoveRange(oldActors);
                    await UnitOfWork.CommitAsync();

                    foreach (var item in Actors)
                    {
                        var actorsImgName = Guid.NewGuid().ToString()
                    + Path.GetExtension(item.FileName);
                        var actorsImgPath = Path.Combine(Directory.GetCurrentDirectory()
                            , "wwwroot/assets/img/ActorsImg", actorsImgName);
                        using (var system = System.IO.File.Create(actorsImgPath))
                        {
                            await item.CopyToAsync(system);
                        }

                        await UnitOfWork.ActorImgRapositoy.CreateAsync(new()
                        {
                            ActorImg = actorsImgName,
                            MovieId = moviesVm.Movie.Id
                        }, cancellationToken);
                    }
                    await UnitOfWork.CommitAsync();
                }

            }
            return "Actor Edit ";
        }
        //Delete
        public string? DeleteActor(Movie movie)
        {
            if (movie.Actors != null && movie.Actors.Any())
            {
                foreach (var actor in movie.Actors)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot/assets/img/ActorsImg", actor.ActorImg);

                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }
                //_context.Actors.RemoveRange(movie.Actors);

                UnitOfWork.ActorImgRapositoy.RemoveRange(movie.Actors);
            }
            return "Actors Img Delete";
        }

        public string? DeleteSupImg(Movie movie)
        {
            if (movie.SupImgs != null && movie.SupImgs.Any())
            {
                foreach (var sup in movie.SupImgs)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot/assets/img/MovieSupImg", sup.Img);

                    if (System.IO.File.Exists(path))
                        System.IO.File.Delete(path);
                }
                //_context.supImgs.RemoveRange(movie.SupImgs);
                UnitOfWork.SupImgRapositoy.RemoveRange(movie.SupImgs);

            }
            return "Sup Img Delete";
        }

        public string? DeleteMainImg(Movie movie)
        {
            if (!string.IsNullOrEmpty(movie.MainImg))
            {
                var oldImgPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot/assets/img/MovieMainImg", movie.MainImg);

                if (System.IO.File.Exists(oldImgPath))
                    System.IO.File.Delete(oldImgPath);
            }
            return "Main Img Delete";
        }
    }
}
