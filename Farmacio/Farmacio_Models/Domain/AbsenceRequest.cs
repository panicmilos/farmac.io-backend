using System;

namespace Farmacio_Models.Domain
{
    public class AbsenceRequest : BaseEntity
    {
        public Guid RequesterId { get; set; }
        public virtual MedicalStaff Requester { get; set; }
        public AbsenceType Type { get; set; }
        public AbsenceRequestStatus Status { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Answer { get; set; }
        public Guid PharmacyId { get; set; }
        public virtual Pharmacy Pharmacy { get; set; }
    }

    public enum AbsenceType
    {
        Absence,
        Vacation
    }

    public enum AbsenceRequestStatus
    {
        Accepted,
        Refused,
        WaitingForAnswer
    }
}
