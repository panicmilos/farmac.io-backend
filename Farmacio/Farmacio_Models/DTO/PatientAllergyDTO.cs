using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Models.DTO
{
    public class PatientAllergyDTO
    {
        public Guid patientId { get; set; }
        public List<Guid> medicinesId { get; set; }
    }
}
