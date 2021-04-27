using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Implementation
{
    public class ComplaintAboutPharmacistService : ComplaintService<ComplaintAboutPharmacist>, IComplaintAboutPharmacistService
    {
        public ComplaintAboutPharmacistService(IRepository<ComplaintAboutPharmacist> repository) :
            base(repository)
        {
        }

        public IEnumerable<Pharmacist> ReadThatPatientCanComplaintAbout(Guid patientId)
        {
            throw new NotImplementedException();
        }
    }
}