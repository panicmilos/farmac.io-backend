using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public abstract class MedicalStaff : User, IGradeable
    {
        public List<Appointment> Appointments { get; set; }
        public int Grade { get; set; }
    }
}
