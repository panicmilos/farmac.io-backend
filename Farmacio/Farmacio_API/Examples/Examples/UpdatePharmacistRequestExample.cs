using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_API.Contracts.Requests.Addresses;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Farmacio_API.Examples.Examples
{
    public class UpdatePharmacistRequestExample : IExamplesProvider<UpdatePharmacistRequest>
    {
        public UpdatePharmacistRequest GetExamples()
        {
            return new UpdatePharmacistRequest
            {
                Account = new UpdateAccountRequest()
                {
                    Id = new Guid("b471c7a4-cad5-40ba-8929-d3c8bc850378")
                },
                User = new UpdatePharmacistUserRequest
                {
                    FirstName = "Luka",
                    LastName = "Bjelica",
                    DateOfBirth = DateTime.Now.AddYears(-21),
                    PID = "1231231231234",
                    PhoneNumber = "0648556699",
                    Address = new UpdateAddressRequest
                    {
                        City = "Zrenjanin",
                        State = "Srbija",
                        StreetName = "Sarajevska",
                        StreetNumber = "52",
                        Lat = 45.392430F,
                        Lng = 20.396100F
                    },
                    PharmacyId = new Guid("08d8ef95-ec90-4a19-8bb3-2e37ea275133")
                }
            };
        }
    }
}