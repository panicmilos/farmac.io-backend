using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts.Repositories;
using Farmacio_Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Farmacio_Services.Implementation
{
    public class MedicineService : CrudService<Medicine>, IMedicineService
    {
        public MedicineService(IRepository<Medicine> repository):
            base(repository)
        {
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
    }
}
