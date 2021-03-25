using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_API.Contracts.Requests.Addresses;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Farmacio_API.Examples.Examples
{
    public class CreatePharmacistRequestExample : IExamplesProvider<CreatePharmacistRequest>
    {
        public CreatePharmacistRequest GetExamples()
        {
            return new CreatePharmacistRequest
            {
                Account = new CreateAccountRequest
                {
                    Email = "bjelicaluka@gmail.com",
                    Username = "beli",
                    Password = "b3l!1234"
                },
                User = new CreatePharmacistUserRequest
                {
                    FirstName = "Luka",
                    LastName = "Bjelica",
                    DateOfBirth = DateTime.Now.AddYears(-21),
                    PID = "1231231232224",
                    PhoneNumber = "0692663554",
                    Address = new CreateAddressRequest
                    {
                        City = "Metropola Zrenjanin",
                        State = "Srbija",
                        StreetName = "Pariske Komune",
                        StreetNumber = "28",
                        Lat = 45.392430F,
                        Lng = 20.396100F
                    },
                    PharmacyId = new Guid("08d8ef95-ec90-4a19-8bb3-2e37ea275133")
                }
            };
        }
    }
}