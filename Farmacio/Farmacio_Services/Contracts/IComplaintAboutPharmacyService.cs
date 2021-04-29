using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IComplaintAboutPharmacyService : IComplaintService<ComplaintAboutPharmacy>
    {
        IEnumerable<Pharmacy> ReadThatPatientCanComplaintAbout(Guid patientId);
    }
}