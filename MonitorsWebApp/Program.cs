using Microsoft.EntityFrameworkCore;
using MonitorsApp.BLC;
using MonitorsApp.Interfaces;

namespace MonitorsApp
{
    public class MonitorsApp
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var daoLibraryName = builder.Configuration.GetSection("DAOLibrary:Path").Value;
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            BLC.BLC blc = new BLC.BLC(daoLibraryName, connectionString);
            builder.Services.AddSingleton(blc);

            // Add services to the container.
            builder.Services.AddRazorPages();

            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}