using Lab15_StudentPortalWeb.Services;
using Microsoft.EntityFrameworkCore;
namespace Lab15_StudentPortalWeb

{
    public static class ID
    {
        public static int id = 34;
        public static int build = (id * 7) + 100;
        public static int lifetime = (id % 3);
        public static string audit_path = $"/audit {id}";
    }
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<StudentPortalContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("StudentPortalContext")));
            builder.Services.AddScoped<IYoussefStampService, YoussefStampService>();
            /*1. Did Stamp A and Stamp B match within a single load?

            Yes. Stamp A and Stamp B matched within the same load because the service is registered as Scoped,
            so both injections use the same service instance within the same HTTP request.

            2. Did the stamps change between loads?

            Yes. The stamps changed between loads because each new HTTP request creates a new Scoped instance of the service,
            and the Stamp is generated in the constructor using Guid.NewGuid().
             */
             
            //builder.Services.AddDbContext<StudentPortalContext>(options =>
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("StudentPortalContext")),ServiceLifetime.Singleton);
            //the app worked normally in singleton , and its bad news because it will not be able to handle multiple requests at the same time,
            //and it will be a bottleneck for the app, so we will use scoped instead of singleton
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

            app.UseAuthorization();

            app.MapStaticAssets();
            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();

            // IMPORTANT NOTE : ALL THE WRITTEN ANSWERS AND CODE SNAPAHOTS ARE IN THE MD FILE WITH THIS REPO,
            // PLEASE CHECK IT OUT FOR MORE DETAILS AND EXPLANATIONS.
        }
    }
}
