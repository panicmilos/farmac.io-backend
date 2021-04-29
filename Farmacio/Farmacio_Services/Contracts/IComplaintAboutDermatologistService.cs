using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IComplaintAboutDermatologistService : IComplaintService<ComplaintAboutDermatologist>
    {
        IEnumerable<Dermatologist> ReadThatPatientCanComplaintAbout(Guid patientId);
    }
}