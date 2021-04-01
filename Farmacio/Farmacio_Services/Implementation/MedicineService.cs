using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class MedicineService : CrudService<Medicine>, IMedicineService
    {
        private readonly IMedicineReplacementService _medicineReplacementService;

        public MedicineService(IMedicineReplacementService medicineReplacementService, IRepository<Medicine> repository) :
            base(repository)
        {
            _medicineReplacementService = medicineReplacementService;
        }

        public FullMedicineDTO Create(FullMedicineDTO fullMedicineDto)
        {
            var medicine = fullMedicineDto.Medicine;
            var replacements = fullMedicineDto.Replacements;

            if (!IsIdUnique(medicine.UniqueId))
            {
                throw new BadLogicException("Code must be unique.");
            }

            foreach (var replacement in replacements)
            {
                if (Read(replacement.ReplacementMedicineId) == null)
                {
                    throw new MissingEntityException("Replacement medicine does not exist in the system.");
                }
            }

            var createdMedicine = base.Create(medicine);
            replacements.ForEach(replacement => replacement.MedicineId = medicine.Id);
            replacements.ForEach(replacement => _medicineReplacementService.Create(replacement));

            return fullMedicineDto;
        }

        public IEnumerable<SmallMedicineDTO> ReadForDisplay()
        {
            return base.Read()
                .ToList()
                .Select(medicine => new SmallMedicineDTO
                {
                    Id = medicine.Id,
                    Name = medicine.Name,
                    Type = medicine.Type,
                    AverageGrade = medicine.AverageGrade,
                    Manufacturer = medicine.Manufacturer
                });
        }

        private bool IsIdUnique(string id)
        {
            return Read().FirstOrDefault(medicine => medicine.UniqueId == id) == default;
        }
    }
}