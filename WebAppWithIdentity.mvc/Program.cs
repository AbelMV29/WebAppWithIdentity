using FluentValidation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
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

            //Add Manager ASP.NET Identity to my services
            builder.Services.AddTransient<UserManager<IdentityUser>>();
            builder.Services.AddTransient<RoleManager<IdentityRole>>();
            builder.Services.AddTransient<SignInManager<IdentityUser>>();

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

            //Add validation of FluentValidation

            

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                            .AddCookie();

            builder.Services.AddAuthorization(op =>
            {

                op.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                op.AddPolicy("User", policy => policy.RequireRole("User"));
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
