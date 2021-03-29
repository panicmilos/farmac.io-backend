using Farmacio_API.Contracts.Requests.Reservations;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace Farmacio_API.Examples.Examples
{
    public class CreateReservationRequestExample : IExamplesProvider<CreateReservationRequest>
    {
        public CreateReservationRequest GetExamples()
        {
            return new CreateReservationRequest
            {
                PatientId = new Guid("d1cb8425-c01f-4552-8660-75910e0def59"),
                PharmacyId = new Guid("874282f3-3df9-4ee0-bf7e-33d9ba3ec456"),
                PickupDeadline = DateTime.Now.AddDays(2),
                Medicines = new List<CreateReservedMedicineRequest>
                {
                    new CreateReservedMedicineRequest
                    {
                        Quantity = 1,
                        MedicineId = new Guid("ce512ff8-3927-43cf-8ae9-33a441b98ea1")
                    },
                    new CreateReservedMedicineRequest
                    {
                        Quantity = 1,
                        MedicineId = new Guid("fe12af31-de77-4f2b-b2a5-11c6d6a340f5")
                    }
                }
            };
        }
    }
}