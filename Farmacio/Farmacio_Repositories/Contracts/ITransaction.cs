using System;

namespace Farmacio_Repositories.Contracts
{
    public interface ITransaction : IDisposable
    {
        public void Commit();

        public void Rollback();
    }
}