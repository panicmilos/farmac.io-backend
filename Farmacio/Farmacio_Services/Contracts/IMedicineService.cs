using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IMedicineService : ICrudService<Medicine>
    {
        FullMedicineDTO Create(FullMedicineDTO fullMedicineDto);

        IEnumerable<SmallMedicineDTO> ReadForDisplay();
    }
}