using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class LoyaltyProgramService : CrudService<LoyaltyProgram>, ILoyaltyProgramService
    {
        private readonly IPatientService _patientService;

        public LoyaltyProgramService(IPatientService patientService, IRepository<LoyaltyProgram> repository) :
            base(repository)
        {
            _patientService = patientService;
        }

        public override LoyaltyProgram Create(LoyaltyProgram loyaltyProgram)
        {
            if (!IsMinimumPointsUnique(loyaltyProgram.MinPoints))
            {
                throw new BadLogicException("Two loyalty programs cannot have same number of minimum points.");
            }

            var createdLoyaltyPoints = base.Create(loyaltyProgram);
            UpdateLoyaltyProgramForAllPatients();

            return createdLoyaltyPoints;
        }

        public override LoyaltyProgram Update(LoyaltyProgram loyaltyProgram)
        {
            var existingLoyaltyProgram = TryToRead(loyaltyProgram.Id);

            if (existingLoyaltyProgram.MinPoints != loyaltyProgram.MinPoints && !IsMinimumPointsUnique(loyaltyProgram.MinPoints))
            {
                throw new BadLogicException("Two loyalty programs cannot have same number of minimum points.");
            }

            existingLoyaltyProgram.Name = loyaltyProgram.Name;
            existingLoyaltyProgram.MinPoints = loyaltyProgram.MinPoints;
            existingLoyaltyProgram.Discount = loyaltyProgram.Discount;

            var updatedLoyaltyProgram = base.Update(existingLoyaltyProgram);
            UpdateLoyaltyProgramForAllPatients();

            return updatedLoyaltyProgram;
        }

        public override LoyaltyProgram Delete(Guid id)
        {
            TryToRead(id);

            var deletedLoyaltyProgram = base.Delete(id);
            UpdateLoyaltyProgramForAllPatients();

            return deletedLoyaltyProgram;
        }

        private void UpdateLoyaltyProgramForAllPatients()
        {
            _patientService.Read().ToList().ForEach(patientAccount => UpdateLoyaltyProgramFor(patientAccount));
        }

        public Account UpdateLoyaltyProgramFor(Account patientAccount)
        {
            var loyaltyPrograms = Read().OrderByDescending(loyaltyProgram => loyaltyProgram.MinPoints);
            if (!loyaltyPrograms.Any())
            {
                return patientAccount;
            }

            var patient = patientAccount.User as Patient;
            var appropriateLoyaltyProgram = loyaltyPrograms.FirstOrDefault(loyaltyProgram => patient.Points >= loyaltyProgram.MinPoints);
            return _patientService.UpdateLoyaltyProgram(patientAccount.Id, appropriateLoyaltyProgram?.Id ?? null);
        }

        private bool IsMinimumPointsUnique(int minPoints)
        {
            return Read().All(loyaltyProgram => loyaltyProgram.MinPoints != minPoints);
        }

        public int ReadDiscountFor(Guid patientId)
        {
            var patientAccount = _patientService.ReadByUserId(patientId);
            if (patientAccount == null)
            {
                throw new MissingEntityException("Given user doesn't exist in the system.");
            }

            var patient = patientAccount.User as Patient;

            return patient?.LoyaltyProgramId == null ? 0 : patient.LoyaltyProgram.Discount;
        }
    }
}