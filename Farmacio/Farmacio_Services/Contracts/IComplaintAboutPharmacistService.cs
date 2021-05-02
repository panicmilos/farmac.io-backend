using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IComplaintAboutPharmacistService : IComplaintService<ComplaintAboutPharmacist>
    {
        IEnumerable<Pharmacist> ReadThatPatientCanComplaintAbout(Guid patientId);
    }
}