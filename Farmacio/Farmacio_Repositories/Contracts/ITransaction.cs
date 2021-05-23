using System;

namespace Farmacio_Repositories.Contracts
{
    public interface ITransaction : IDisposable
    {
        void Commit();

        void Rollback();
    }
}