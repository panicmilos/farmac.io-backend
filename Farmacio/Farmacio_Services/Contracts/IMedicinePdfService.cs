using Farmacio_Models.DTO;
using System.IO;

namespace Farmacio_Services.Contracts
{
    public interface IMedicinePdfService
    {
        void GenerateFor(FullMedicineDTO medicine);

        FileStream GetPdfStreamFor(FullMedicineDTO medicine);

        void DeleteFor(FullMedicineDTO medicine);
    }
}