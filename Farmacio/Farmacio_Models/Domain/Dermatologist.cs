using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class Dermatologist : MedicalStaff
    {
        public List<DermatologistWorkPlace> WorkPlaces { get; set; }
    }

    public class DermatologistWorkPlace
    {
        public WorkTime WorkTime { get; set; }
        public Pharmacy Pharmacy { get; set; }
    }
}
