using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PlatformService.Models;

namespace PlatformService.Data{
    public static class PrepDb{
        public static void PrepPopulation(IApplicationBuilder app, bool isProd){
            using( var serviceScope = app.ApplicationServices.CreateScope()){
                SeeData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProd);
            }
        }
        private static void SeeData(AppDbContext context, bool isProd){

            if(isProd){
                System.Console.WriteLine("--> Migrations...");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"Not run Migrations {ex.Message}");;
                }
            }

            if(!context.Platforms.Any()){
                Console.WriteLine("See all data");
                context.Platforms.AddRange(
                    new Platform() {Name = "DotNet", Publisher = "Microsoft", Cost = "Free"},
                    new Platform() {Name = "Sql Server", Publisher = "Microsoft", Cost = "Free"},
                    new Platform() {Name = "Docker", Publisher = "Cloud Native Computing Foundation", Cost = "Free"} 

                );
                context.SaveChanges();
            }
            else{
                Console.WriteLine("already have data");
            }
        }
    }
}