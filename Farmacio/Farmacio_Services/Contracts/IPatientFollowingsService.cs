using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IPatientFollowingsService
    {
        PatientPharmacyFollow Read(Guid followId);

        IEnumerable<Account> ReadFollowersFor(Guid pharmacyId);

        PatientPharmacyFollow Follow(Guid patientAccountId, Guid pharmacyId);

        IEnumerable<PatientPharmacyFollow> ReadFollowingsOf(Guid patientAccountId);

        PatientPharmacyFollow Unfollow(Guid followId);
    }
}