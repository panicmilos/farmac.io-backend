using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts.Repositories;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;

namespace Farmacio_Services.Implementation
{
    public class DermatologistService : AccountService, IDermatologistService
    {
        private readonly IPharmacyService _pharmacyService;
        
        public DermatologistService(IEmailVerificationService emailVerificationService, IPharmacyService pharmacyService
            , IRepository<Account> repository) 
            : base(emailVerificationService, repository)
        {
            _pharmacyService = pharmacyService;
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

        public IEnumerable<Account> ReadForPharmacy(Guid pharmacyId)
        {
            return FilterByPharmacyId(Read(), pharmacyId);
        }

        public Account ReadForPharmacy(Guid pharmacyId, Guid dermatologistId)
        {
            var account = Read(dermatologistId);
            var dermatologist = (Dermatologist) account?.User;
            return dermatologist?.WorkPlaces.FirstOrDefault(wp => wp.Pharmacy.Id == pharmacyId) != null
                ? account
                : null;
        }

        public IEnumerable<Account> SearchByNameForPharmacy(Guid pharmacyId, string name)
        {
            return FilterByPharmacyId(SearchByName(name), pharmacyId);
        }

        public Account AddToPharmacy(Guid pharmacyId, Guid dermatologistId, WorkTime workTime)
        {
            var account = Read(dermatologistId);
            if(account == null)
                throw new MissingEntityException("Dermatologist with the given Id not found.");

            var pharmacy = _pharmacyService.Read(pharmacyId);
            if(pharmacy == null)
                throw new MissingEntityException("Pharmacy with the given Id not found.");
            
            var dermatologist = (Dermatologist) account.User;
            var workPlace = dermatologist.WorkPlaces.FirstOrDefault(wp => wp.Pharmacy.Id == pharmacyId);
            
            if(workPlace != null)
                throw new AlreadyEmployedInPharmacyException("Dermatologist already employed in pharmacy.");

            ValidateWorkTime(workTime);

            if(!IsWorkTimeForDermatologistValid(workTime, dermatologist))
                throw new WorkTimesOverlapException("Work time overlaps with another.");
            
            var newWorkPlace = new DermatologistWorkPlace
            {
                Pharmacy = pharmacy,
                WorkTime = workTime
            };
            
            dermatologist.WorkPlaces.Add(newWorkPlace);
            
            return Update(account);
        }

        public Account RemoveFromPharmacy(Guid pharmacyId, Guid dermatologistId)
        {
            var account = Read(dermatologistId);
            if(account == null)
                throw new MissingEntityException("Dermatologist with the given Id not found.");
            
            var dermatologist = (Dermatologist) account.User;
            var workPlace = dermatologist.WorkPlaces.FirstOrDefault(wp => wp.Pharmacy.Id == pharmacyId);
            
            if(workPlace == null)
                throw new MissingEntityException("Dermatologist is not employed in the given pharmacy.");
            
            dermatologist.WorkPlaces.Remove(workPlace);
            return Update(account);
        }

        private static IEnumerable<Account> FilterByPharmacyId(IEnumerable<Account> accounts, Guid pharmacyId)
        {
            return accounts.Where(d =>
                ((Dermatologist) d.User).WorkPlaces.FirstOrDefault(wp => wp.Pharmacy.Id == pharmacyId) != null);
        }
        
        private static void ValidateWorkTime(WorkTime workTime)
        {
            var workTimeHourDiff = Math.Abs(workTime.From.Hour - workTime.To.Hour);
            var workTimeMinuteDiff = Math.Abs(workTime.From.Minute - workTime.To.Minute);
            if (workTimeHourDiff < 1 || workTimeHourDiff > 8 || (workTimeHourDiff == 8 && workTimeMinuteDiff != 0))
                throw new InvalidWorkTimeException("Work time must be minimum 1 hour and maximum 8 hours long.");
        }
        
        private static bool IsWorkTimeForDermatologistValid(WorkTime workTime, Dermatologist dermatologist)
        {
            var overlap = dermatologist.WorkPlaces.FirstOrDefault(wp =>
            {
                var hoursFrom = wp.WorkTime.From.Hour;
                var minutesFrom = wp.WorkTime.From.Minute;
                var hoursTo = wp.WorkTime.To.Hour;
                var minutesTo = wp.WorkTime.To.Minute;

                var isBefore = workTime.To.Hour < hoursFrom ||
                               (workTime.To.Hour == hoursFrom && workTime.To.Minute <= minutesFrom);

                var isAfter = workTime.From.Hour >= hoursTo ||
                              (workTime.From.Hour == hoursTo && workTime.From.Minute >= minutesTo);

                return !(isBefore || isAfter);
            });
            return overlap == null;
        }
    }
}