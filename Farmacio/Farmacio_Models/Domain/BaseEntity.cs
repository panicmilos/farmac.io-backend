using System;
using Farmacio_Models.Contracts;

namespace Farmacio_Models.Domain
{
    public abstract class BaseEntity : IEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool Active { get; set; } = true;
    }
}
