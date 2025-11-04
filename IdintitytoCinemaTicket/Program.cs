using IdintitytoCinemaTicket.Data;
using IdintitytoCinemaTicket.Models;
using IdintitytoCinemaTicket.Reposatory;
using IdintitytoCinemaTicket.Reposatory.IRepositories;
using IdintitytoCinemaTicket.Serviece;
using IdintitytoCinemaTicket.Serviece.IServiece;
using IdintitytoCinemaTicket.UnitOfWork;
using IdintitytoCinemaTicket.UnitOfWork;
using IdintitytoCinemaTicket.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;


namespace IdintitytoCinemaTicket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>();
            builder.Services.AddScoped<IRepository<Cinema>, Repository<Cinema>>();
            builder.Services.AddScoped<IRepository<Category>, Repository<Category>>();
            builder.Services.AddScoped<IRepository<Movie>, Repository<Movie>>();
            builder.Services.AddScoped<IActorImgRapositoy, ActorImgRapositoy>();
            builder.Services.AddScoped<IMovieImgRapositoy, MovieImgRapositoy>();
            builder.Services.AddScoped<IUnitOfWork, IdintitytoCinemaTicket.UnitOfWork.UnitOfWork>();
            builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddTransient<IEmailSender, EmailSendr>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IRepository<ApplicationUserOTP>, Repository<ApplicationUserOTP>>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>
                (
                option =>
                {
                    option.Password.RequiredLength = 8;
                    option.User.RequireUniqueEmail = true;
                    option.SignIn.RequireConfirmedEmail = true;
                }
                )
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            // External Login With Google
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = "Google";
            })
             .AddGoogle("Google", opt =>
             {
                 var googleAuth = builder.Configuration.GetSection("Authentication:Google");
                 opt.ClientId = googleAuth["ClientId"] ?? "";
                 opt.ClientSecret = googleAuth["ClientSecret"] ?? "";
                 opt.SignInScheme = IdentityConstants.ExternalScheme;
             });
            // External Login With FaceBook

            builder.Services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"] ?? "";
                facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"] ?? "";
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Identity}/{controller=Account}/{action=Login}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
