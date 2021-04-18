using System;
using System.Collections.Generic;

namespace Farmacio_Models.DTO
{
    public class PatientAllergyDTO
    {
        public Guid patientId { get; set; }
        public List<Guid> medicineIds { get; set; }
    }
}
