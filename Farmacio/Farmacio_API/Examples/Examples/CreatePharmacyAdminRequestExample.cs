using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_API.Contracts.Requests.Addresses;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Farmacio_API.Examples.Examples
{
    public class CreatePharmacyAdminRequestExample : IExamplesProvider<CreatePharmacyAdminRequest>
    {
        public CreatePharmacyAdminRequest GetExamples()
        {
            return new CreatePharmacyAdminRequest
            {
                Account = new CreateAccountRequest
                {
                    Username = "jojev",
                    Password = "!@j0j3v@!",
                    Email = "joleikompanija@gmail.com"
                },
                User = new CreatePharmacyAdminUserRequest
                {
                    FirstName = "Jovana",
                    LastName = "Jeftic",
                    DateOfBirth = DateTime.Now.AddYears(-20),
                    PID = "1231231231231",
                    PhoneNumber = "0414214213",
                    Address = new CreateAddressRequest
                    {
                        State = "Srbija",
                        City = "Mali zvornik",
                        StreetName = "Milos Gajica",
                        StreetNumber = "23",
                        Lat = 44.375554F,
                        Lng = 19.107461F
                    },
                    PharmacyId = new Guid("08d8ef95-ec90-4a19-8bb3-2e37ea275133")
                }
            };
        }
    }
}