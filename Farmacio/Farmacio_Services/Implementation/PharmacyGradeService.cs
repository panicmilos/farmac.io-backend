﻿using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Implementation.Utils;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class PharmacyGradeService : GradeService, IPharmacyGradeService
    {
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IReservationService _reservationService;
        private readonly IPharmacyService _pharmacyService;
        private readonly IERecipeService _eRecipeService;

        public PharmacyGradeService(IRepository<Grade> repository,
            IPatientService patientService,
            IAppointmentService appointmentService,
            IReservationService reservationService,
            IPharmacyService pharmacyService,
            IERecipeService eRecipeService) :
            base(repository)
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
            _reservationService = reservationService;
            _pharmacyService = pharmacyService;
            _eRecipeService = eRecipeService;
        }

        public override Grade Create(Grade grade)
        {
            if (_patientService.ReadByUserId(grade.PatientId) == null)
            {
                throw new MissingEntityException("The given patient does not exist in the system.");
            }

            var pharmacyGrade = grade as PharmacyGrade;

            var pharmacy = _pharmacyService.TryToRead(pharmacyGrade.PharmacyId);

            if (!HasConsumedServicesOfPharmacyInPast(grade.PatientId, pharmacyGrade.PharmacyId))
            {
                throw new BadLogicException("Patient didn't consume services of given pharmacy in the past.");
            }

            if (DidPatientRatePharmacy(grade.PatientId, pharmacyGrade.PharmacyId))
            {
                throw new BadLogicException("The patient has already rated the pharmacy.");
            }

            if (grade.Value < 1 || grade.Value > 5)
            {
                throw new BadLogicException("The grade can have a value between 1 and 5.");
            }

            pharmacy.AverageGrade = (pharmacy.AverageGrade * pharmacy.NumberOfGrades + pharmacyGrade.Value) / ++pharmacy.NumberOfGrades;
            base.Create(pharmacyGrade);
            _pharmacyService.UpdateGrade(pharmacy);

            return pharmacyGrade;
        }

        private bool HasConsumedServicesOfPharmacyInPast(Guid patientId, Guid pharmacyId)
        {
            var hasDoneReservation = _reservationService.ReadFor(patientId)
                .FirstOrDefault(reservation => reservation.State == ReservationState.Done &&
                                               reservation.PharmacyId == pharmacyId) != null;

            if (hasDoneReservation) return true;

            var hasConsultationOrExaminationInPast = _appointmentService.ReadForPatient(patientId)
                .FirstOrDefault(appointment => appointment.PharmacyId == pharmacyId &&
                                               appointment.DateTime < DateTime.Now &&
                                               appointment.IsReserved) != null;

            return hasConsultationOrExaminationInPast;
        }

        private bool DidPatientRatePharmacy(Guid patientId, Guid pharmacyId)
        {
            var grades = Read().ToList().Where(grade =>
            {
                var pharmacyGrade = grade as PharmacyGrade;
                return pharmacyGrade?.PharmacyId == pharmacyId && pharmacyGrade?.PatientId == patientId;
            });
            return grades?.FirstOrDefault() != null;
        }

        public IEnumerable<Pharmacy> ReadThatPatientCanRate(Guid patientId)
        {
            var pharmaciesThatPatientCanRate = new List<Pharmacy>();

            pharmaciesThatPatientCanRate.AddRange(
                _reservationService.ReadFor(patientId)
                    .Where(reservation => reservation.State == ReservationState.Done)
                    .Select(reservation => reservation.Pharmacy));

            pharmaciesThatPatientCanRate.AddRange(
                _appointmentService.ReadForPatient(patientId)
                    .Where(appointment => appointment.DateTime < DateTime.Now &&
                                          appointment.IsReserved)
                    .Select(appointment => appointment.Pharmacy));


            return pharmaciesThatPatientCanRate.ToHashSet().Where(pharmacy => !DidPatientRatePharmacy(patientId, pharmacy.Id));
        }

        public PharmacyGrade ChangeGrade(Guid pharmacyGradeId, int value)
        {
            var pharmacyGrade = TryToRead(pharmacyGradeId) as PharmacyGrade;

            var pharmacy = _pharmacyService.TryToRead(pharmacyGrade.PharmacyId);

            if(value < 1 || value > 5)
            {
                throw new BadLogicException("The grade can be between 0 and 5.");
            }

            if(pharmacyGrade.Value == value)
            {
                return pharmacyGrade;
            }

            pharmacy.AverageGrade = (pharmacy.AverageGrade * pharmacy.NumberOfGrades - pharmacyGrade.Value + value) / pharmacy.NumberOfGrades;
            _pharmacyService.UpdateGrade(pharmacy);

            pharmacyGrade.Value = value;

            return base.Update(pharmacyGrade) as PharmacyGrade;
        }

        public IEnumerable<Pharmacy> ReadPharmaciesThatPatientRated(Guid patientId)
        {
            return _pharmacyService.Read().ToList().Where(pharmacy => DidPatientRatePharmacy(patientId, pharmacy.Id)).ToList();
        }

        public PharmacyGrade Read(Guid patientId, Guid pharmacyId)
        {
            return Read().FirstOrDefault(grade =>
            {
                var pharmacyGrade = grade as PharmacyGrade;
                return pharmacyGrade?.PatientId == patientId && pharmacyGrade?.PharmacyId == pharmacyId;
            }) as PharmacyGrade;
        }

        public IEnumerable<Pharmacy> ReadPharmaciesThatPatientRatedPageTo(Guid patientId, PageDTO pageDTO)
        {
            return PaginationUtils<Pharmacy>.Page(ReadPharmaciesThatPatientRated(patientId), pageDTO);
        }
    }
}
