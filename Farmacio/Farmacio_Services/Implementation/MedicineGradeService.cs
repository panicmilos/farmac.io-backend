using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Implementation.Utils;
using GlobalExceptionHandler.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class MedicineGradeService : GradeService, IMedicineGradeService
    {
        private readonly IReservationService _reservationService;
        private readonly IERecipeService _eRecipeService;
        private readonly IMedicineService _medicineService;
        private readonly IPatientService _patientService;

        public MedicineGradeService(IRepository<Grade> repository, IReservationService reservationService, IERecipeService eRecipeService, IMedicineService medicineService,
            IPatientService patientService):
            base(repository)
        {
            _eRecipeService = eRecipeService;
            _reservationService = reservationService;
            _medicineService = medicineService;
            _patientService = patientService;
        }

        public MedicineGrade ChangeGrade(Guid medicineGradeId, int value)
        {
            using var transaction = _repository.OpenTransaction();
            try
            {
                var medicineGrade = TryToRead(medicineGradeId) as MedicineGrade;

                var medicine = _medicineService.TryToRead(medicineGrade.MedicineId);

                if (value < 1 || value > 5)
                {
                    throw new BadLogicException("The grade can be between 0 and 5.");
                }

                if (medicineGrade.Value == value)
                {
                    return medicineGrade;
                }

                medicine.AverageGrade = (medicine.AverageGrade * medicine.NumberOfGrades - medicineGrade.Value + value) / medicine.NumberOfGrades;
                _medicineService.UpdateGrade(medicine);

                medicineGrade.Value = value;

                var updatedGrade = base.Update(medicineGrade) as MedicineGrade;

                transaction.Commit();
                return updatedGrade;
            }
            catch (DbUpdateConcurrencyException)
            {
                transaction.Rollback();
                throw new BadLogicException("Something bad happened. Please try again.");
            }
        }

        public override Grade Create(Grade grade)
        {
            using var transaction = _repository.OpenTransaction();
            try
            {
                var medicineGrade = grade as MedicineGrade;

                var medicine = _medicineService.TryToRead(medicineGrade.MedicineId);

                var patient = _patientService.ReadByUserId(medicineGrade.PatientId);

                if (patient == null)
                {
                    throw new MissingEntityException("The given patient does not exist in the system.");
                }

                if (!_reservationService.DidPatientReserveMedicine(medicineGrade.MedicineId, medicineGrade.PatientId) &&
                    !_eRecipeService.WasMedicinePrescribedToPatient(medicineGrade.PatientId, medicineGrade.MedicineId))
                {
                    throw new BadLogicException("The patient can rate the medicine if he has reserved and taken the medicine or if it has been prescribed to him.");
                }

                if (grade.Value < 1 || grade.Value > 5)
                {
                    throw new BadLogicException("The grade can have a value between 1 and 5.");
                }

                if (DidPatientRateMedicine(medicineGrade.MedicineId, medicineGrade.PatientId))
                {
                    throw new BadLogicException("The patient has already rated the medicine.");
                }

                medicine.AverageGrade = (medicine.AverageGrade * medicine.NumberOfGrades + medicineGrade.Value) / ++medicine.NumberOfGrades;
                base.Create(medicineGrade);
                _medicineService.UpdateGrade(medicine);

                transaction.Commit();
                return medicineGrade;
            }
            catch (DbUpdateConcurrencyException)
            {
                transaction.Rollback();
                throw new BadLogicException("Something bad happened. Please try again.");
            }
        }

        public IEnumerable<Medicine> ReadThatPatientCanRate(Guid patientId)
        {
            var medicines = _medicineService.Read().ToList().Where(medicine => _reservationService.DidPatientReserveMedicine(medicine.Id, patientId)).ToList();
            foreach (var medicine in _medicineService.Read().ToList())
            {
                if (medicines.All(reservedMedicine => reservedMedicine.Id != medicine.Id) && _eRecipeService.WasMedicinePrescribedToPatient(medicine.Id, patientId))
                {
                    medicines.Add(medicine);
                }
            }
            return medicines.Where(medicine => !DidPatientRateMedicine(medicine.Id, patientId));
        }

        public IEnumerable<Medicine> ReadMedicinesThatPatientRated(Guid patientId)
        {
            return _medicineService.Read().ToList().Where(medicine => DidPatientRateMedicine(medicine.Id, patientId)).ToList();
        }

        private bool DidPatientRateMedicine(Guid medicineId, Guid patientId)
        {
            var grades = Read().ToList().Where(grade =>
            {
                var medicineGrade = grade as MedicineGrade;
                return medicineGrade?.MedicineId == medicineId && medicineGrade?.PatientId == patientId;
            }).ToList();
            return grades?.FirstOrDefault() != null;
        }

        public MedicineGrade Read(Guid patientId, Guid medicineId)
        {
            return Read().Where(grade =>
            {
                var medicineGrade = grade as MedicineGrade;
                return medicineGrade?.PatientId == patientId && medicineGrade?.MedicineId == medicineId;
            }).FirstOrDefault() as MedicineGrade;
        }

        public IEnumerable<Medicine> ReadMedicinesThatPatientRatedPageTo(Guid patientId, PageDTO pageDTO)
        {
            return PaginationUtils<Medicine>.Page(ReadMedicinesThatPatientRated(patientId), pageDTO);
        }
    }
}
