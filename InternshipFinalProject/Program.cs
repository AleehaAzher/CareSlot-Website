using InternshipFinalProject_Infrastructure.DataAccess;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace InternshipFinalProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();
            builder.Services.AddPresentationDI();
            builder.Services.AddDbContext<DataAccessClass>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));
            });
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/User/Login";
                options.AccessDeniedPath = "/Home/AccessDenied";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            });
            builder.Services.AddAuthorization();
            builder.Services.AddHttpContextAccessor();
            var app = builder.Build();

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
                pattern: "{controller=Home}/{action=SignUp}/{id?}");

            app.Run();
        }
    }
}
