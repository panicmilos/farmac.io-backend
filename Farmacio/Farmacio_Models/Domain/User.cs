using System;

namespace Farmacio_Models.Domain
{
    public abstract class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PID { get; set; }
        public string PhoneNumber { get; set; }
        public Guid AddressId { get; set; }
        public virtual Address Address { get; set; }
    }
}
