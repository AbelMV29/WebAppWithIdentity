using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAppWithIdentity.mvc.Data;
using WebAppWithIdentity.mvc.Helpers;
using WebAppWithIdentity.mvc.Interfaces;
using WebAppWithIdentity.mvc.Models;
using WebAppWithIdentity.mvc.Repository;
using WebAppWithIdentity.mvc.Services;

namespace WebAppWithIdentity.mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
            builder.Services.AddScoped<IPhotoService, PhotoService>();
            


            //Add mi Database and bind the connectionString
            builder.Services.AddDbContext<DefaultDb>(op =>
            {
                op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            //Add options configurations of CloudinarySettings to PhotoService
            builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

            //Add IdentityUser and IdentityRole for autentication and authorization
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<DefaultDb>();

            builder.Services.AddTransient<UserManager<IdentityUser>>();
            builder.Services.AddTransient<RoleManager<IdentityRole>>();
            builder.Services.AddTransient<SignInManager<IdentityUser>>();

            builder.Services.AddAuthorization(op =>
            {
                op.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                op.AddPolicy("User", policy => policy.RequireRole("User"));
            });

            var app = builder.Build();

            if(args.Length > 0 && args[0] == "seed")
            {

            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
