using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public abstract class MedicalStaff : User, IGradeable
    {
        public List<Appointment> Appointments { get; set; }
        public List<AbsenceRequest> AbsenceRequests { get; set; }
        public int AverageGrade { get; set; }
        public List<Grade> Grades { get; set; }
    }
}
