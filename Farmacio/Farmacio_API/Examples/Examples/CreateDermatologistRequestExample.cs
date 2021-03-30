using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_API.Contracts.Requests.Addresses;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Farmacio_API.Examples.Examples
{
    public class CreateDermatologistRequestExample : IExamplesProvider<CreateDermatologistRequest>
    {
        public CreateDermatologistRequest GetExamples()
        {
            return new CreateDermatologistRequest
            {
                Account = new CreateAccountRequest
                {
                    Username = "nemanjicjr",
                    Password = "nem@njic123",
                    Email = "nemanjicjr@gmail.com"
                },
                User = new CreateDermatologistUserRequest
                {
                    FirstName = "Vanja",
                    LastName = "Nemanjic",
                    DateOfBirth = DateTime.Now.AddYears(-14),
                    PID = "1231231231231",
                    PhoneNumber = "0690905010",
                    Address = new CreateAddressRequest
                    {
                        City = "Zrenjanin",
                        State = "Srbija",
                        StreetName = "Sarajevska",
                        StreetNumber = "52",
                        Lat = 45.392430F,
                        Lng = 20.396100F
                    }
                }
            };
        }
    }
}