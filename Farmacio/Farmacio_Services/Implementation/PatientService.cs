using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class PatientService : AccountService, IPatientService
    {
        public PatientService(IEmailVerificationService emailVerificationService,
            IRepository<Account> repository)
            : base(emailVerificationService, repository)
        {
        }

        public bool HasExceededLimitOfNegativePoints(Guid patientId)
        {
            var account = base.Read().Where(account => account.UserId == patientId).FirstOrDefault();
            if (account == null)
            {
                throw new BadLogicException("The given patient does not exist in the system.");
            }
            var patient = (Patient)account.User;
            return patient.NegativePoints >= 3;
        }

        public override IEnumerable<Account> Read()
        {
            return base.Read().Where(account => account.Role == Role.Patient).ToList();
        }

        public override Account Read(Guid id)
        {
            var account = base.Read(id);
            return account?.Role == Role.Patient ? account : null;
        }

        public override Account TryToRead(Guid id)
        {
            var existingAccount = Read(id);
            if (existingAccount == null)
                throw new MissingEntityException("Patient account not found.");
            return existingAccount;
        }

        public void DeleteNegativePoints()
        {
            foreach (var patientAccount in Read())
            {
                var patient = patientAccount.User as Patient;
                patient.NegativePoints = 0;
                base.Update(patientAccount);
            }
        }

        public Account UpdateLoyaltyProgram(Guid patientAccountId, Guid? loyaltyProgramId)
        {
            var patientAccount = TryToRead(patientAccountId);
            var patient = patientAccount.User as Patient;

            patient.LoyaltyProgramId = loyaltyProgramId;

            return base.Update(patientAccount);
        }

        public Account UpdateLoyaltyPoints(Guid patientUserId, int forPoints)
        {
            var patientAccount = ReadByUserId(patientUserId);
            if (patientAccount == null)
            {
                throw new MissingEntityException("Patient doesn't exist in the system.");
            }

            var patient = patientAccount.User as Patient;
            patient.Points += forPoints;

            return base.Update(patientAccount);
        }
    }
}