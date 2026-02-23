using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Souq.Data;
using Souq.Models;

namespace Souq
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ===============================
            // Database
            // ===============================
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            builder.Services.AddDbContext<SouqcomContext>(options =>
    options.UseSqlServer(connectionString));


            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            // ===============================
            // Identity
            // ===============================
            builder.Services.AddDefaultIdentity<IdentityUser>(options =>
                options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            // ===============================
            // MVC + Session
            // ===============================
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddSession();

            var app = builder.Build();

            // ===============================
            // Middleware Pipeline
            // ===============================
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();   // مهم جداً
            app.UseAuthorization();

            app.UseSession();

            // ===============================
            // Routing
            // ===============================
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();

            app.Run();
        }
    }
}
