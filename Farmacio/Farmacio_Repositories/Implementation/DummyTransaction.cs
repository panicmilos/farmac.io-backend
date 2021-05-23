using Farmacio_Repositories.Contracts;

namespace Farmacio_Repositories.Implementation
{
    public class DummyTransaction : ITransaction
    {
        public DummyTransaction()
        {
        }

        public void Commit()
        {
        }

        public void Rollback()
        {
        }

        public void Dispose()
        {
        }
    }
}