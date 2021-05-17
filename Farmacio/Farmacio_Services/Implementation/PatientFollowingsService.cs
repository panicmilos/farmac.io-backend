using Farmacio_Models.Domain;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class PatientFollowingsService : IPatientFollowingsService
    {
        private readonly ICrudService<PatientPharmacyFollow> _followingsService;
        private readonly IPatientService _patientService;
        private readonly IPharmacyService _pharmacyService;

        public PatientFollowingsService(
            ICrudService<PatientPharmacyFollow> followsService,
            IPatientService patientService,
            IPharmacyService pharmacyService
            )
        {
            _followingsService = followsService;
            _patientService = patientService;
            _pharmacyService = pharmacyService;
        }

        public IEnumerable<Account> ReadFollowersFor(Guid pharmacyId)
        {
            return _followingsService.Read().Where(follow => follow.PharmacyId == pharmacyId)
                .ToList()
                .Select(follow => _patientService.ReadByUserId(follow.PatientId));
        }

        public PatientPharmacyFollow Follow(Guid patientAccountId, Guid pharmacyId)
        {
            var patient = _patientService.TryToRead(patientAccountId);
            _pharmacyService.TryToRead(pharmacyId);

            if (DoesPatientFollowPharmacy(patient.UserId, pharmacyId))
            {
                throw new BadLogicException("Patient already follow given pharmacy.");
            }

            return _followingsService.Create(new PatientPharmacyFollow
            {
                PatientId = patient.UserId,
                PharmacyId = pharmacyId
            });
        }

        private bool DoesPatientFollowPharmacy(Guid patientId, Guid pharmacyId)
        {
            return _followingsService.Read().Where(follow => follow.PatientId == patientId && follow.PharmacyId == pharmacyId).FirstOrDefault() != null;
        }

        public IEnumerable<PatientPharmacyFollow> ReadFollowingsOf(Guid patientAccountId)
        {
            var patient = _patientService.TryToRead(patientAccountId);

            return _followingsService.Read().Where(follow => follow.PatientId == patient.UserId).ToList();
        }

        public PatientPharmacyFollow Unfollow(Guid followId)
        {
            _followingsService.TryToRead(followId);

            return _followingsService.Delete(followId);
        }

        public PatientPharmacyFollow Read(Guid followId)
        {
            return _followingsService.TryToRead(followId);
        }
    }
}