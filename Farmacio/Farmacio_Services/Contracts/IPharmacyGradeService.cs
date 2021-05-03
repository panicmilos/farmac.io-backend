using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Services.Contracts
{
    public interface IPharmacyGradeService : IGradeService
    {
        IEnumerable<Pharmacy> ReadThatPatientCanRate(Guid patientId);
    }
}
