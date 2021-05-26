using System;
using Farmacio_API.Extensions;
using Farmacio_Repositories.Implementation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Farmacio_API
{
    public static class Program
    {
        private static readonly bool RunMigrations = Environment.GetEnvironmentVariable("RunMigrations") == "true";
        private static readonly bool SeedDatabase = Environment.GetEnvironmentVariable("SeedDatabase") == "true";

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            if (RunMigrations)
                host.MigrateDbContext<DatabaseContext>();
            if (SeedDatabase)
                host.SeedDbContext<DatabaseContext>();
            host.Run();
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