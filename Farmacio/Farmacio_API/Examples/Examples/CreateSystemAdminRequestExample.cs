using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_API.Contracts.Requests.Addresses;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Farmacio_API.Examples.Examples
{
    public class CreateSystemAdminRequestExample : IExamplesProvider<CreateSystemAdminRequest>
    {
        public CreateSystemAdminRequest GetExamples()
        {
            return new CreateSystemAdminRequest
            {
                Account = new CreateAccountRequest
                {
                    Username = "admin",
                    Password = "@dmin123",
                    Email = "adminadminic@gmail.com"
                },
                User = new CreateSystemAdminUserRequest
                {
                    FirstName = "Admin",
                    LastName = "Adminic",
                    DateOfBirth = DateTime.Now.AddYears(-32),
                    PID = "0001112223334",
                    PhoneNumber = "0000000000",
                    Address = new CreateAddressRequest
                    {
                        State = "Srbija",
                        City = "Novi Sad",
                        StreetName = "Bulevar Despota Stefana",
                        StreetNumber = "7",
                        Lat = 45.236300F,
                        Lng = 19.838230F
                    }
                }
            };
        }
    }
}