﻿using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;
using Farmacio_Services.Implementation.Utils;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Models.DTO;
using Farmacio_Services.Implementation.Validation;

namespace Farmacio_Services.Implementation
{
    public class DermatologistService : MedicalStaffService, IDermatologistService
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly IDermatologistWorkPlaceService _dermatologistWorkPlaceService;
        private readonly IAppointmentService _appointmentService;
        private readonly IMedicalStaffGradeService _medicalStaffGradeService;

        public DermatologistService(IEmailVerificationService emailVerificationService, IPharmacyService pharmacyService
            , IDermatologistWorkPlaceService dermatologistWorkPlaceService, IAppointmentService appointmentService
            , IMedicalStaffGradeService medicalStaffGradeService
            , IRepository<Account> repository)
            : base(emailVerificationService, appointmentService, repository)
        {
            _pharmacyService = pharmacyService;
            _dermatologistWorkPlaceService = dermatologistWorkPlaceService;
            _appointmentService = appointmentService;
            _medicalStaffGradeService = medicalStaffGradeService;
        }

        public override IEnumerable<Account> Read()
        {
            return base.Read().Where(account => account.Role == Role.Dermatologist).ToList();
        }

        public override Account Read(Guid id)
        {
            var account = base.Read(id);

            return account?.Role == Role.Dermatologist ? account : null;
        }

        public override Account TryToRead(Guid id)
        {
            var existingAccount = Read(id);
            if (existingAccount == null)
                throw new MissingEntityException("Dermatologist account not found.");
            return existingAccount;
        }

        public IEnumerable<Account> ReadForPharmacy(Guid pharmacyId)
        {
            return FilterByPharmacyId(Read(), pharmacyId);
        }

        public Account ReadForPharmacy(Guid pharmacyId, Guid dermatologistAccountId)
        {
            var dermatologistAccount = Read(dermatologistAccountId);
            return _dermatologistWorkPlaceService
                .GetWorkPlaceInPharmacyFor(dermatologistAccount.UserId, pharmacyId) != null
                ? dermatologistAccount
                : null;
        }

        public IEnumerable<Account> SearchByNameForPharmacy(Guid pharmacyId, string name)
        {
            return FilterByPharmacyId(SearchByName(name), pharmacyId);
        }

        public override IEnumerable<Account> ReadBy(MedicalStaffFilterParamsDTO filterParams)
        {
            var pharmacyId = filterParams.PharmacyId;
            return pharmacyId != null
                ? FilterByPharmacyId(base.ReadBy(filterParams), new Guid(pharmacyId))
                : base.ReadBy(filterParams);
        }

        public Account AddToPharmacy(Guid pharmacyId, Guid dermatologistAccountId, WorkTime workTime)
        {
            var dermatologistAccount = TryToRead(dermatologistAccountId);
            var pharmacy = _pharmacyService.TryToRead(pharmacyId);

            var workPlace =
                _dermatologistWorkPlaceService.GetWorkPlaceInPharmacyFor(dermatologistAccount.UserId, pharmacyId);

            if (workPlace != null)
                throw new AlreadyEmployedInPharmacyException("Dermatologist already employed in pharmacy.");

            WorkTimeValidation.ValidateWorkHours(workTime);
            
            if (!IsWorkTimeForDermatologistValid(workTime, dermatologistAccount.UserId))
                throw new WorkTimesOverlapException("Work time overlaps with another.");

            var newWorkPlace = new DermatologistWorkPlace
            {
                DermatologistId = dermatologistAccount.UserId,
                Pharmacy = pharmacy,
                WorkTime = workTime
            };

            _dermatologistWorkPlaceService.Create(newWorkPlace);

            return Update(dermatologistAccount);
        }

        public Account RemoveFromPharmacy(Guid pharmacyId, Guid dermatologistAccountId)
        {
            var dermatologistAccount = TryToRead(dermatologistAccountId);

            var workPlace =
                _dermatologistWorkPlaceService.GetWorkPlaceInPharmacyFor(dermatologistAccount.UserId, pharmacyId);
            if (workPlace == null)
                throw new NotEmployedInPharmacyException("Dermatologist is not employed in the given pharmacy.");

            _dermatologistWorkPlaceService.Delete(workPlace.Id);
            return dermatologistAccount;
        }

        private IEnumerable<Account> FilterByPharmacyId(IEnumerable<Account> accounts, Guid pharmacyId)
        {
            return accounts.Where(a =>
                _dermatologistWorkPlaceService.GetWorkPlaceInPharmacyFor(a.UserId, pharmacyId) != null);
        }

        private bool IsWorkTimeForDermatologistValid(WorkTime workTime, Guid dermatologistId)
        {
            var overlap = _dermatologistWorkPlaceService
                .GetWorkPlacesFor(dermatologistId)
                .FirstOrDefault(wp => TimeIntervalUtils
                    .TimeIntervalTimesOverlap(wp.WorkTime.From, wp.WorkTime.To, workTime.From, workTime.To));
            return overlap == null;
        }

        public Grade GradeDermatologist(MedicalStaffGrade grade)
        {
            var dermatologist = ReadByUserId(grade.MedicalStaffId);
            if(dermatologist == null)
            {
                throw new MissingEntityException("The given dermatologist does not exist.");
            }

            if (!_appointmentService.DidPatientHaveAppointmentWithDermatologist(grade.PatientId, grade.MedicalStaffId))
            {
                throw new BadLogicException("The patient cannot rate the dermatologist because he did not have an appointment with him.");
            }

            if (_medicalStaffGradeService.DidPatientGradeMedicalStaff(grade.PatientId, grade.MedicalStaffId))
            {
                throw new BadLogicException("The patient has already been rate a dermatologist.");
            }

            if (grade.Value < 1 || grade.Value > 5)
            {
                throw new BadLogicException("The score can have a value between 1 and 5");
            }

            grade = _medicalStaffGradeService.Create(grade) as MedicalStaffGrade;
            var medicalStaff = dermatologist.User as MedicalStaff;
            medicalStaff.AverageGrade = (medicalStaff.NumberOfGrades * medicalStaff.AverageGrade + grade.Value) / ++medicalStaff.NumberOfGrades;
            dermatologist.User = medicalStaff;
            base.UpdateGrade(medicalStaff);

            return grade;
        }

        public IEnumerable<Account> ReadThatPatientCanRate(Guid patientId)
        {
            return Read().Where(dermatologist => _appointmentService.DidPatientHaveAppointmentWithDermatologist(patientId, dermatologist.UserId) &&
            !_medicalStaffGradeService.DidPatientGradeMedicalStaff(patientId, dermatologist.UserId)).ToList();
        }

        public IEnumerable<Account> ReadThatPatientRated(Guid patientId)
        {
            return Read().Where(dermatologist => _medicalStaffGradeService.DidPatientGradeMedicalStaff(patientId, dermatologist.UserId)).ToList();
        }
    }
}