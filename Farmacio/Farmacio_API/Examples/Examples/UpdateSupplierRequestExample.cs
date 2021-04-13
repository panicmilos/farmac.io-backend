using Farmacio_API.Contracts.Requests.Accounts;
using Farmacio_API.Contracts.Requests.Addresses;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace Farmacio_API.Examples.Examples
{
    public class UpdateSupplierRequestExample : IExamplesProvider<UpdateSupplierRequest>
    {
        public UpdateSupplierRequest GetExamples()
        {
            return new UpdateSupplierRequest
            {
                Account = new UpdateAccountRequest
                {
                    Id = new Guid("08d8fac7-3089-407b-8ef9-4b554714b1e3")
                },
                User = new UpdateSupplierUserRequest
                {
                    FirstName = "Ebay",
                    LastName = "Profa",
                    DateOfBirth = DateTime.Now.AddYears(-25),
                    PID = "9999991111110",
                    PhoneNumber = "0691414140",
                    Address = new UpdateAddressRequest
                    {
                        State = "Srbija",
                        City = "Novi Sad",
                        StreetName = "Bulevar Despota Stefana",
                        StreetNumber = "7",
                        Lat = 45.236300F,
                        Lng = 19.838230F
                    }
                }
            };
        }
    }
}