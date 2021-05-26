using Farmacio_Repositories.Contracts;

namespace Farmacio_Repositories.Implementation
{
    public class DummyTransaction : ITransaction
    {
        public void Commit()
        {
            // This method is empty because it is dummy transaction.
        }

        public void Rollback()
        {
            // This method is empty because it is dummy transaction.
        }

        public void Dispose()
        {
            // This method is empty because it is dummy transaction.
        }
    }
}