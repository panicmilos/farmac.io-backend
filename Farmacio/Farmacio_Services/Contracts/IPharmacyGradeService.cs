using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Services.Contracts
{
    public interface IPharmacyGradeService : IGradeService
    {
        IEnumerable<Pharmacy> ReadThatPatientCanRate(Guid patientId);
        PharmacyGrade ChangeGrade(Guid pharmacyGradeId, int value);
        IEnumerable<Pharmacy> ReadPharmaciesThatPatientRated(Guid patientId);
        PharmacyGrade Read(Guid patientId, Guid pharmacyId);
    }
}
