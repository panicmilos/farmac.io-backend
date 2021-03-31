using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_API.Contracts.Requests.Addresses;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Farmacio_API.Examples.Examples
{
    public class UpdateSystemAdminRequestExample : IExamplesProvider<UpdateSystemAdminRequest>
    {
        public UpdateSystemAdminRequest GetExamples()
        {
            return new UpdateSystemAdminRequest
            {
                Account = new UpdateAccountRequest
                {
                    Id = new Guid("08d8f393-8534-4312-8793-34151df972e3")
                },
                User = new UpdateSystemAdminUserRequest
                {
                    FirstName = "Admin",
                    LastName = "Adminic",
                    DateOfBirth = DateTime.Now.AddYears(-36),
                    PID = "0011223344556",
                    PhoneNumber = "0622342341",
                    Address = new UpdateAddressRequest
                    {
                        State = "Srbija",
                        City = "Novi Sad",
                        StreetName = "Bulevar Despota Stefana Liman",
                        StreetNumber = "7b",
                        Lat = 45.236300F,
                        Lng = 19.838230F
                    }
                }
            };
        }
    }
}