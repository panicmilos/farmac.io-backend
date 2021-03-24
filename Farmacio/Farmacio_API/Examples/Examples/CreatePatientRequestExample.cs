using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_API.Contracts.Requests.Addresses;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Farmacio_API.Examples.Examples
{
    public class CreatePatientRequestExample : IExamplesProvider<CreatePatientRequest>
    {
        public CreatePatientRequest GetExamples()
        {
            return new CreatePatientRequest
            {
                Account = new CreateAccountRequest
                {
                    Email = "panic.milos99@gmail.com",
                    Username = "panic",
                    Password = "panic123"
                },
                User = new CreatePatientUserRequest
                {
                    FirstName = "Milos",
                    LastName = "Panic",
                    DateOfBirth = DateTime.Now,
                    PID = "1231231231234",
                    PhoneNumber = "0692506099",
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