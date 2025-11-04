using IdintitytoCinemaTicket.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdintitytoCinemaTicket.Data
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public DbSet<Movie> movies { get; set; }
        public DbSet<Cinema> cinemas { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<MoviesSupImg> supImgs { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<ApplicationUserOTP> applicationUserOTPs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Idintitytocinema;" +
                "Integrated Security=True;Connect Timeout=30;Encrypt=True;" +
                "Trust Server Certificate=True;");
        }
    }
}
