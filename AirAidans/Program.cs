//using Air_Aidan_s.DATA.EF;
using AirAidans.Data;
using AirAidans.DATA.EF.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;

namespace AirAidans
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            //Registering our new GadgetStore Database Context
            builder.Services.AddDbContext<AirAidansContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount =
                true).AddRoles<IdentityRole>().AddRoleManager<RoleManager<IdentityRole>>
                ().AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();

            #region Locker (a.k.a Shopping Cart)
            builder.Services.AddSession(options =>
            {
                //the duration a session is stored in memory
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                //Allows us to set cookie options over unsecure connections 
                options.Cookie.HttpOnly = true;
                //Cannot be declined.
                options.Cookie.IsEssential = true;
            });
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
            app.MapRazorPages();

            app.Run();
        }
    }
}