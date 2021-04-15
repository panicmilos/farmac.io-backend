﻿using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmacio_Services.Implementation
{
    public class PatientAllergyService : CrudService<PatientAllergy>, IPatientAllergyService
    {
        private readonly IMedicineService _medicineService;
        private readonly IPatientService _patientService;

        public PatientAllergyService(IRepository<PatientAllergy> repository, IMedicineService medicineService, IPatientService patientService):
            base(repository)
        {
            _medicineService = medicineService;
            _patientService = patientService;
        }

        public IEnumerable<PatientAllergy> Create(PatientAllergyDTO request)
        {
            _patientService.TryToRead(request.patientId);
            foreach(var medicineId in request.medicinesId)
            {
                _medicineService.TryToRead(medicineId);
                var allergy = base.Read().Where(a => a.MedicineId == medicineId && a.PatientId == request.patientId).FirstOrDefault();
                if (allergy != null)
                {
                    throw new BadLogicException("Allergy already exists in the system.");
                }
            }

            List<PatientAllergy> listOfAllergies = new List<PatientAllergy>();
            foreach(var medicineId in request.medicinesId)
            {
                listOfAllergies.Add(base.Create(new PatientAllergy
                {
                    MedicineId = medicineId,
                    PatientId = request.patientId
                }));
            }
            return listOfAllergies;
        }
    }
}