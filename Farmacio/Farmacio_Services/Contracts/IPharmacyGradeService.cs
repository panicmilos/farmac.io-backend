using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IPharmacyGradeService : IGradeService
    {
        IEnumerable<Pharmacy> ReadThatPatientCanRate(Guid patientId);

        PharmacyGrade ChangeGrade(Guid pharmacyGradeId, int value);

        IEnumerable<Pharmacy> ReadPharmaciesThatPatientRated(Guid patientId);

        PharmacyGrade Read(Guid patientId, Guid pharmacyId);

        IEnumerable<Pharmacy> ReadPharmaciesThatPatientRatedPageTo(Guid patientId, PageDTO pageDTO);
    }
}
