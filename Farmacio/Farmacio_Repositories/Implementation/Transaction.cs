using Farmacio_Repositories.Contracts;
using Microsoft.EntityFrameworkCore.Storage;

namespace Farmacio_Repositories.Implementation
{
    public class Transaction : ITransaction
    {
        private readonly IDbContextTransaction _transaction;
        private readonly DatabaseContext _context;

        public Transaction(DatabaseContext context)
        {
            _transaction = context.Database.BeginTransaction();
            _context = context;
            _context.IsTransactionOpened = true;
        }

        public void Commit()
        {
            _transaction.Commit();
            _context.IsTransactionOpened = false;
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _context.IsTransactionOpened = false;
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
}