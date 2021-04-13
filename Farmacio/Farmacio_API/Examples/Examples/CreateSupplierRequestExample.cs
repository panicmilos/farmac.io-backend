using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_API.Contracts.Requests.Addresses;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Farmacio_API.Examples.Examples
{
    public class CreateSupplierRequestExample : IExamplesProvider<CreateSupplierRequest>
    {
        public CreateSupplierRequest GetExamples()
        {
            return new CreateSupplierRequest
            {
                Account = new CreateAccountRequest
                {
                    Username = "theebay",
                    Password = "123eb@y321",
                    Email = "theproebay@gmail.com"
                },
                User = new CreateSupplierUserRequest
                {
                    FirstName = "Ebay",
                    LastName = "Counter",
                    DateOfBirth = DateTime.Now.AddYears(-20),
                    PID = "0000000000000",
                    PhoneNumber = "0691515150",
                    Address = new CreateAddressRequest
                    {
                        State = "Srbija",
                        City = "Novi Sad",
                        StreetName = "Bulevar Despota Stefana",
                        StreetNumber = "7a",
                        Lat = 45.236300F,
                        Lng = 19.838230F
                    }
                }
            };
        }
    }
}