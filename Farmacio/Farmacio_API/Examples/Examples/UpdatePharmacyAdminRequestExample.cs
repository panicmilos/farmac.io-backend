using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_API.Contracts.Requests.Addresses;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Farmacio_API.Examples.Examples
{
    public class UpdatePharmacyAdminRequestExample : IExamplesProvider<UpdatePharmacyAdminRequest>
    {
        public UpdatePharmacyAdminRequest GetExamples()
        {
            return new UpdatePharmacyAdminRequest
            {
                Account = new UpdateAccountRequest
                {
                    Id = new Guid("08d8efd8-c9c0-40d7-8e34-c7a0f497a6f3")
                },
                User = new UpdatePharmacyAdminUserRequest
                {
                    FirstName = "Jovanaa",
                    LastName = "Jeftic",
                    DateOfBirth = DateTime.Now.AddYears(-22),
                    PID = "1231231231230",
                    PhoneNumber = "0214214213",
                    Address = new UpdateAddressRequest
                    {
                        State = "Srbijaa",
                        City = "Mali zvornikk",
                        StreetName = "Milos Gajicaa",
                        StreetNumber = "233",
                        Lat = 42.375554F,
                        Lng = 17.107461F
                    },
                    PharmacyId = new Guid("874282f3-3df9-4ee0-bf7e-33d9ba3ec456")
                }
            };
        }
    }
}