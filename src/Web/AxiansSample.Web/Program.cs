using AxiansSample.Web.Models;
using AxiansSample.Web.Services;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;

namespace AxiansSample.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Adding the model
            builder.Services.AddSingleton(new AxiansSampleDbConext());
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

            // Add SignalR
            builder.Services.AddSignalR();

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
                pattern: "{controller=Home}/{action=Index}/{id?}");
            
            // Register SignalR endpoint
            app.MapHub<IncidentHub>("/incidentHub");
            app.Run();
        }
    }
}
