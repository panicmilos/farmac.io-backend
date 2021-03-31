using System;
using Farmacio_API.Contracts.Requests.Addresses;

namespace Farmacio_API.Contracts.Requests.Pharmacies
{
    public class UpdatePharmacyRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CreateAddressRequest Address { get; set; }
    }
}