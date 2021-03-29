using Farmacio_API.Extensions;
using Farmacio_Repositories.Implementation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Farmacio_API
{
    public class SeedDb
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
                .Build()
                .MigrateDbContext<DatabaseContext>()
                .SeedDbContext<DatabaseContext>()
                .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                        .ConfigureWebHostDefaults(webBuilder =>
                        {
                            webBuilder.UseStartup<Startup>();
                        });
        }
    }
}