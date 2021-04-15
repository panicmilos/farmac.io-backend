using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_API.Contracts.Requests.Addresses;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Farmacio_API.Examples.Examples
{
    public class UpdatePatientRequestExample : IExamplesProvider<UpdatePatientRequest>
    {
        public UpdatePatientRequest GetExamples()
        {
            return new UpdatePatientRequest
            {
                Account = new UpdateAccountRequest
                {
                    Id = new Guid("08d8fae0-4104-4c69-8b05-54b9bf3acd7b")
                },
                User = new UpdatePatientUserRequest
                {
                    FirstName = "Miloss",
                    LastName = "Panicc",
                    DateOfBirth = DateTime.Now.AddYears(-22),
                    PID = "0001112223331",
                    PhoneNumber = "0692506099",
                    Address = new UpdateAddressRequest
                    {
                        City = "Zrenjanin",
                        State = "Srbija",
                        StreetName = "Sarajevska",
                        StreetNumber = "52a",
                        Lat = 44.392430F,
                        Lng = 19.396100F
                    }
                }
            };
        }
    }
}