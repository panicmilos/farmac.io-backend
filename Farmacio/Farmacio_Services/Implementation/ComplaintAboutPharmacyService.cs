using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Implementation
{
    public class ComplaintAboutPharmacyService : ComplaintService<ComplaintAboutPharmacy>, IComplaintAboutPharmacyService
    {
        public ComplaintAboutPharmacyService(IRepository<ComplaintAboutPharmacy> repository) :
            base(repository)
        {
        }

        public IEnumerable<Pharmacy> ReadThatPatientCanComplaintAbout(Guid patientId)
        {
            throw new NotImplementedException();
        }
    }
}