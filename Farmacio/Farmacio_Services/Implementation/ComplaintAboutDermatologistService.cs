using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Implementation
{
    public class ComplaintAboutDermatologistService : ComplaintService<ComplaintAboutDermatologist>, IComplaintAboutDermatologistService
    {
        public ComplaintAboutDermatologistService(IRepository<ComplaintAboutDermatologist> repository) :
            base(repository)
        {
        }

        public IEnumerable<Dermatologist> ReadThatPatientCanComplaintAbout(Guid patientId)
        {
            throw new NotImplementedException();
        }
    }
}