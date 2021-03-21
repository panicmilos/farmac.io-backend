using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public abstract class MedicalStaff : User, IGradeable
    {
        public virtual List<Appointment> Appointments { get; set; }
        public virtual List<AbsenceRequest> AbsenceRequests { get; set; }
        public int AverageGrade { get; set; }
        public virtual List<Grade> Grades { get; set; }
    }
}
