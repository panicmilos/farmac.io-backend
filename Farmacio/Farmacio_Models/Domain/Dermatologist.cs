using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public class Dermatologist : MedicalStaff
    {
        public virtual List<DermatologistWorkPlace> WorkPlaces { get; set; }
    }

    public class DermatologistWorkPlace
    {
        public virtual WorkTime WorkTime { get; set; }
        public virtual Pharmacy Pharmacy { get; set; }
    }
}
