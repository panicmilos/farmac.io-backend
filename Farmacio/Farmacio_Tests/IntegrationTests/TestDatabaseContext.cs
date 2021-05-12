using Farmacio_API.Settings;
using Farmacio_Repositories.Implementation;
using Microsoft.EntityFrameworkCore;

namespace Farmacio_Tests.IntegrationTests
{
    public class TestDatabaseContext : DatabaseContext
    {
        public TestDatabaseContext() :
            base(new DbContextOptions<DatabaseContext>())
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbSettings = new DatabaseSettings
            {
                Server = "bjelicaluka.com",
                Port = "3310",
                Database = "farmacio_db_tests",
                User = "root",
                Password = "1234"
            };

            optionsBuilder.UseLazyLoadingProxies()
                          .UseMySql(dbSettings.GetConnectionString(),
                                b => b.MigrationsAssembly("Farmacio_Tests"));
        }
    }
}