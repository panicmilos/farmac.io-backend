using System;

namespace Farmacio_Models.Contracts
{
    public interface IEntity
    {
        Guid Id { get; set; }
        DateTime CreatedAt { get; set; }
        bool Active { get; set; }
    }
}
