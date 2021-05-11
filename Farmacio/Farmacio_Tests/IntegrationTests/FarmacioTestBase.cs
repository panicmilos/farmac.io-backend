using Farmacio_Repositories.Implementation;
using System;

namespace Farmacio_Tests.IntegrationTests
{
    public abstract class FarmacioTestBase : IDisposable
    {
        protected DatabaseContext context;

        public FarmacioTestBase()
        {
            context = new TestDatabaseContext();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}