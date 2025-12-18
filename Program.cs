using Microsoft.EntityFrameworkCore;
using School_of_arts.ModelBinding;

namespace School_of_arts
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<School_of_arts.Models.SchoolOfArtsContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            builder.Services.AddControllersWithViews(o =>
            {
                o.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
            });
            var supportedCultures = new[] { new System.Globalization.CultureInfo("uk-UA") };
            var app = builder.Build();
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("uk-UA"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
