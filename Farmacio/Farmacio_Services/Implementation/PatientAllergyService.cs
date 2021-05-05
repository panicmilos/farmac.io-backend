using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public IEnumerable<CheckMedicineDTO> ForEachMedicineInListCheckIfPatientHasAnAllergyToIt(IEnumerable<CheckMedicineDTO> medicineDTOs, Guid patientId)
        {
            var account = _patientService.ReadByUserId(patientId);
            _patientService.TryToRead(account.Id);
            var allergies = GetPatientsAllergies(account.Id).Select(m => m.Id);
            foreach (var medicineDTO in medicineDTOs)
                medicineDTO.IsAllergy = allergies.Contains(medicineDTO.MedicineId);
            return medicineDTOs;
        }

        public IEnumerable<PatientAllergy> CreateAllergies(PatientAllergyDTO request)
        {
            _patientService.TryToRead(request.patientId);
            foreach(var medicineId in request.medicineIds)
            {
                _medicineService.TryToRead(medicineId);
                var allergy = base.Read().Where(a => a.MedicineId == medicineId && a.PatientId == request.patientId).FirstOrDefault();
                if (allergy != null)
                {
                    throw new BadLogicException("Allergy already exists in the system.");
                }
            }

            List<PatientAllergy> listOfAllergies = new List<PatientAllergy>();
            foreach(var medicineId in request.medicineIds)
            {
                listOfAllergies.Add(base.Create(new PatientAllergy
                {
                    MedicineId = medicineId,
                    PatientId = request.patientId
                }));
            }
            return listOfAllergies;
        }

        public IEnumerable<SmallMedicineDTO> GetPatientsAllergies(Guid patientId)
        {
            _patientService.TryToRead(patientId);
            return base.Read().ToList().Where(allergy => allergy.PatientId == patientId)
                .Select(allergy => new SmallMedicineDTO
                {
                    Name = allergy.Medicine.Name,
                    AverageGrade = allergy.Medicine.AverageGrade,
                    Manufacturer = allergy.Medicine.Manufacturer,
                    Type = allergy.Medicine.Type,
                    Id = allergy.Medicine.Id
                });
        }

        public PatientAllergy Delete(Guid patientId, Guid medicineId)
        {
            _patientService.TryToRead(patientId);
            _medicineService.TryToRead(medicineId);

            var allergy = Read().FirstOrDefault(allergy => allergy.MedicineId == medicineId && allergy.PatientId == patientId);
            if(allergy == null)
            {
                throw new BadLogicException("The given allergy does not exist in the system.");
            }

            return base.Delete(allergy.Id);
        }
    }
}
