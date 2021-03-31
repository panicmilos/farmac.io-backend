using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_API.Contracts.Requests.Addresses;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Farmacio_API.Examples.Examples
{
    public class UpdateDermatologistRequestExample : IExamplesProvider<UpdateDermatologistRequest>
    {
        public UpdateDermatologistRequest GetExamples()
        {
            return new UpdateDermatologistRequest
            {
                Account = new UpdateAccountRequest
                {
                    Id = new Guid("08d8f38a-47b3-494e-8e68-44010f4aa169")
                },
                User = new UpdateDermatologistUserRequest
                {
                    FirstName = "Vanja",
                    LastName = "Nemanjic",
                    DateOfBirth = DateTime.Now.AddYears(-16),
                    PID = "0123012301231",
                    PhoneNumber = "0690952010",
                    Address = new UpdateAddressRequest
                    {
                        City = "Zrenjanin",
                        State = "Srbija",
                        StreetName = "Sarajevska",
                        StreetNumber = "52a",
                        Lat = 45.392430F,
                        Lng = 20.396100F
                    }
                }
            };
        }
    }
}