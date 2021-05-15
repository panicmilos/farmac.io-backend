﻿using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Implementation
{
    public class MedicalStaffService : AccountService, IMedicalStaffService
    {
        private readonly IAppointmentService _appointmentService;

        public MedicalStaffService(IEmailVerificationService emailVerificationService, IAppointmentService appointmentService,
            IAccountRepository repository)
            : base(emailVerificationService, repository)
        {
            _appointmentService = appointmentService;
        }

        public IEnumerable<PatientDTO> ReadPatientsForMedicalStaff(Guid medicalAccountId)
        {
            var medicalAccount = TryToRead(medicalAccountId);

            return _appointmentService
                .ReadForMedicalStaff(medicalAccount.UserId)
                .Where(appointment => appointment.IsReserved && appointment.PatientId != null)
                .GroupBy(appointment => appointment.PatientId)
                .Select(group => group.Where(appointment => appointment.DateTime == group.Max(appointment => appointment.DateTime)).First())
                .Select(appointment => new PatientDTO
                {
                    PatientId = appointment.PatientId.Value,
                    FirstName = appointment.Patient.FirstName,
                    LastName = appointment.Patient.LastName,
                    DateOfBirth = appointment.Patient.DateOfBirth,
                    Address = appointment.Patient.Address,
                    PhoneNumber = appointment.Patient.PhoneNumber,
                    AppointmentDate = appointment.DateTime
                });
        }

        public IEnumerable<PatientDTO> ReadSortedPatientsForMedicalStaff(Guid medicalAccountId, string crit, bool isAsc)
        {
            var sortingCriteria = new Dictionary<string, Func<PatientDTO, object>>()
            {
                { "firstName", p => p.FirstName },
                { "lastName", p => p.LastName },
                { "appointmentDate", p => p.AppointmentDate }
            };
            var patients = ReadPatientsForMedicalStaff(medicalAccountId);
            if (sortingCriteria.TryGetValue(crit ?? "", out var sortCrit))
                patients = isAsc ? patients.OrderBy(sortCrit) : patients.OrderByDescending(sortCrit);
            return patients;
        }

        public virtual IEnumerable<Account> ReadBy(MedicalStaffFilterParamsDTO filterParams)
        {
            var (name, pharmacyId, gradeFrom, gradeTo) = filterParams;
            return SearchByName(name)
                .Where(acc =>
                {
                    var dermatologist = (MedicalStaff)acc.User;
                    return (gradeFrom == 0 || dermatologist.AverageGrade >= gradeFrom)
                           && (gradeTo == 0 || dermatologist.AverageGrade <= gradeTo);
                });
        }

        public IEnumerable<PatientDTO> SearchPatientsForMedicalStaff(Guid medicalAccountId, string name)
        {
            return ReadPatientsForMedicalStaff(medicalAccountId).Where(p =>
                name == null ||
                $"{p.FirstName.ToLower()} {p.LastName.ToLower()}".Contains(name.ToLower()));
        }

        public Account UpdateGrade(MedicalStaff medicalStaff)
        {
            var staffAccount = ReadByUserId(medicalStaff.Id);
            var staffUser = staffAccount.User as MedicalStaff;
            staffUser.NumberOfGrades = medicalStaff.NumberOfGrades;
            staffUser.AverageGrade = medicalStaff.AverageGrade;
            staffAccount.User = staffUser;
            return base.Update(staffAccount);
        }
    }
}