using Farmacio_API.Contracts.Requests.Addresses;
using System;

namespace Farmacio_API.Contracts.Requests.Accounts
{
    public class CreateUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PID { get; set; } //?
        public string PhoneNumber { get; set; }
        public CreateAddressRequest Address { get; set; }
    }
}