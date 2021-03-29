using System;
using Farmacio_Models.Domain;

namespace Farmacio_Models.DTO
{
    public class PatientDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public Address Address { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
