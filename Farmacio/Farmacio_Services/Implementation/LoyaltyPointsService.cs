using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using System;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class LoyaltyPointsService : CrudService<LoyaltyPoints>, ILoyaltyPointsService
    {
        private readonly IMedicineService _medicineService;
        private readonly ICrudService<MedicinePoints> _medicinePointsService;
        private readonly IPatientService _patientService;

        public LoyaltyPointsService(
            IMedicineService medicineService,
            ICrudService<MedicinePoints> medicinePointsService,
            IPatientService patientService,
            IRepository<LoyaltyPoints> repository
        ) :
            base(repository)
        {
            _medicineService = medicineService;
            _medicinePointsService = medicinePointsService;
            _patientService = patientService;
        }

        public LoyaltyPoints ReadOrCreate()
        {
            var loyaltyPoints = Read();
            if (loyaltyPoints.Count() == 0)
            {
                Create(new LoyaltyPoints());
            }

            return Read().ToList().First();
        }

        public int ReadConsultationPoints()
        {
            var loyaltyPoints = ReadOrCreate();

            return loyaltyPoints.ConsultationPoints;
        }

        public int ReadExaminationPoints()
        {
            var loyaltyPoints = ReadOrCreate();

            return loyaltyPoints.ExaminationPoints;
        }

        public int ReadPointsFor(Guid medicineId)
        {
            var loyaltyPoints = ReadOrCreate();
            var medicinePoints = loyaltyPoints.MedicinePointsList.FirstOrDefault(medicinePoints => medicinePoints.MedicineId == medicineId);

            return medicinePoints?.Points ?? 0;
        }

        public override LoyaltyPoints Update(LoyaltyPoints loyaltyPoints)
        {
            loyaltyPoints.MedicinePointsList.ForEach(medicinePoints => _medicineService.TryToRead(medicinePoints.MedicineId));

            var existingLoyaltyPoints = ReadOrCreate();
            _medicinePointsService.Delete(existingLoyaltyPoints.MedicinePointsList.Select(medicinePoints => medicinePoints.Id));

            existingLoyaltyPoints.ConsultationPoints = loyaltyPoints.ConsultationPoints;
            existingLoyaltyPoints.ExaminationPoints = loyaltyPoints.ExaminationPoints;
            existingLoyaltyPoints.MedicinePointsList = loyaltyPoints.MedicinePointsList;

            return _repository.Update(existingLoyaltyPoints);
        }

        public Account GivePointsFor(Reservation reservation)
        {
            var numberOfPoints = reservation.Medicines.Sum(reservedMedicine => ReadPointsFor(reservedMedicine.MedicineId) * reservedMedicine.Quantity);

            return _patientService.UpdateLoyaltyPoints(reservation.PatientId, numberOfPoints);
        }

        public Account GivePointsFor(Appointment appointment)
        {
            if (appointment.MedicalStaff is Dermatologist)
            {
                return _patientService.UpdateLoyaltyPoints(appointment.PatientId.Value, ReadExaminationPoints());
            }

            return _patientService.UpdateLoyaltyPoints(appointment.PatientId.Value, ReadConsultationPoints());
        }
    }
}