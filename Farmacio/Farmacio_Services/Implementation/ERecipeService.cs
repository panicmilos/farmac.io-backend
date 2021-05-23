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
    public class ERecipeService : CrudService<ERecipe>, IERecipeService
    {
        private readonly IPharmacyService _pharmacyService;
        private readonly IReservationService _reservationService;
        private readonly IPatientService _patientService;

        public ERecipeService(
            IPharmacyService pharmacyService,
            IReservationService reservationService,
            IPatientService patientService,
            IRepository<ERecipe> repository
         ) : base(repository)
        {
            _pharmacyService = pharmacyService;
            _reservationService = reservationService;
            _patientService = patientService;
        }

        public override ERecipe Create(ERecipe eRecipe)
        {
            eRecipe.UniqueId = GetUniqueId();
            return base.Create(eRecipe);
        }

        public override ERecipe Update(ERecipe eRecipe)
        {
            var existingERecipe = TryToRead(eRecipe.Id);
            existingERecipe.IsUsed = eRecipe.IsUsed;

            return _repository.Update(existingERecipe);
        }

        public Reservation CreateReservationFromERecipe(CreateReservationFromERecipeDTO createERecipeDTO)
        {
            var transaction = _repository.OpenTransaction();
            try
            {
                var existingERecipe = TryToRead(createERecipeDTO.ERecipeId);
                _pharmacyService.TryToRead(createERecipeDTO.PharmacyId);

                if (existingERecipe.IsUsed)
                {
                    throw new BadLogicException("ERecipe is already used.");
                }

                var reservation = new Reservation
                {
                    PatientId = existingERecipe.PatientId,
                    PharmacyId = createERecipeDTO.PharmacyId,
                    PickupDeadline = createERecipeDTO.PickupDeadline,
                    Medicines = existingERecipe.Medicines.Select(prescribedMedicine => new ReservedMedicine
                    {
                        MedicineId = prescribedMedicine.MedicineId,
                        Quantity = prescribedMedicine.Quantity
                    }).ToList()
                };

                var createdReservation = _reservationService.CreateReservation(reservation, true);
                existingERecipe.IsUsed = true;
                Update(existingERecipe);

                transaction.Commit();
                return createdReservation;
            }
            catch (DbUpdateConcurrencyException)
            {
                transaction.Rollback();
                throw new BadLogicException("Something bad happend. Please try again.");
            }
        }

        public IEnumerable<PharmacyForERecipeDTO> FindPharmaciesWithMedicinesFrom(Guid eRecipeId)
        {
            var existingERecipe = TryToRead(eRecipeId);

            return _pharmacyService.Read().ToList()
                .Where(pharmacy =>
                {
                    return existingERecipe.Medicines.All(medicine =>
                    {
                        try
                        {
                            var medicineInPharmacy = _pharmacyService.ReadMedicine(pharmacy.Id, medicine.MedicineId);
                            if (medicineInPharmacy.InStock < medicine.Quantity)
                            {
                                return false;
                            }
                            return true;
                        }
                        catch (Exception)
                        {
                            return false;
                        }
                    });
                })
                .Select(pharmacy => new PharmacyForERecipeDTO
                {
                    Id = pharmacy.Id,
                    Name = pharmacy.Name,
                    Address = pharmacy.Address,
                    AverageGrade = pharmacy.AverageGrade,
                    TotalPriceOfMedicines = existingERecipe.Medicines.Sum(medicine => _pharmacyService.ReadMedicine(pharmacy.Id, medicine.MedicineId).Price * medicine.Quantity)
                });
        }

        public IEnumerable<ERecipe> ReadFor(Guid patientId)
        {
            return Read().Where(eRecipe => eRecipe.PatientId == patientId).ToList();
        }

        public IEnumerable<PharmacyForERecipeDTO> SortPharmaciesWithMedicinesFrom(Guid eRecipeId, string sortCriteria, bool isAscending)
        {
            var pharmacies = FindPharmaciesWithMedicinesFrom(eRecipeId);
            var sortingCriteria = new Dictionary<string, Func<PharmacyForERecipeDTO, object>>()
            {
                { "name", p => p.Name },
                { "grade", p => p.AverageGrade },
                { "price", p => p.TotalPriceOfMedicines }
            };

            if (sortingCriteria.TryGetValue(sortCriteria ?? "", out var sortingCriterion))
            {
                pharmacies = isAscending ? pharmacies.OrderBy(sortingCriterion) : pharmacies.OrderByDescending(sortingCriterion);
            }

            return pharmacies;
        }

        public IEnumerable<ERecipeMedicine> ReadMedicinesFromERecipe(Guid eRecipeId)
        {
            var eRecipe = base.TryToRead(eRecipeId);

            return eRecipe.Medicines;
        }

        public IEnumerable<ERecipeDTO> SortFor(Guid patientUserId, ERecipesSortFilterParams sortFilterParams)
        {
            var patient = _patientService.ReadByUserId(patientUserId);

            if (patient == null)
            {
                throw new MissingEntityException("The given patient does not exist in the system.");
            }

            (string sortCriteria, bool isAsc, bool? isUsed) = sortFilterParams;

            var eRecipes = Read().ToList().Where(eRecipe => eRecipe.PatientId == patientUserId);

            if (isUsed.HasValue)
            {
                eRecipes = eRecipes.Where(eRecipe => eRecipe.IsUsed == isUsed);
            }

            var sortingCriteria = new Dictionary<string, Func<ERecipe, object>>()
            {
                { "issuingDate", e => e.IssuingDate }
            };

            if (sortingCriteria.TryGetValue(sortCriteria ?? "", out var sortingCriterion))
            {
                eRecipes = isAsc ? eRecipes.OrderBy(sortingCriterion) : eRecipes.OrderByDescending(sortingCriterion);
            }

            return eRecipes.Select(eRecipe => new ERecipeDTO
            {
                Id = eRecipe.Id,
                IssuingDate = eRecipe.IssuingDate,
                IsUsed = eRecipe.IsUsed,
                UniqueId = eRecipe.UniqueId
            });
        }

        public bool WasMedicinePrescribedToPatient(Guid patientId, Guid medicineId)
        {
            var eRecipes = ReadFor(patientId);
            eRecipes = eRecipes.Where(eRecipe =>
            {
                var medicines = eRecipe.Medicines;
                return medicines.Where(medicine => medicine.MedicineId == medicineId).FirstOrDefault() != null;
            }).ToList();
            return eRecipes.Count() > 0;
        }

        private string GetUniqueId()
        {
            string uniqueId;
            do
            {
                uniqueId = StringUtils.RandomString(10);
            } while (!IsIdUnique(uniqueId));

            return uniqueId;
        }

        private bool IsIdUnique(string id)
        {
            return Read().FirstOrDefault(reservation => reservation.UniqueId == id) == default;
        }

        public IEnumerable<ERecipeDTO> SortForPageTo(Guid patientId, ERecipesSortFilterParams sortFilterParams, PageDTO pageDTO)
        {
            return PaginationUtils<ERecipeDTO>.Page(SortFor(patientId, sortFilterParams), pageDTO);
        }
    }
}