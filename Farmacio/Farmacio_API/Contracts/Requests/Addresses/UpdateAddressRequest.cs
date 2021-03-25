using System;

namespace Farmacio_API.Contracts.Requests.Addresses
{
    public class UpdateAddressRequest : CreateAddressRequest
    {
        public Guid Id { get; set; }
    }
}