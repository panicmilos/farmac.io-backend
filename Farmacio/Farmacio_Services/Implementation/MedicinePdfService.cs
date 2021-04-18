using Farmacio_Models.DTO;
using Farmacio_Services.Contracts;
using IronPdf;
using Scriban;
using System.IO;

namespace Farmacio_Services.Implementation
{
    public class MedicinePdfService : IMedicinePdfService
    {
        private readonly HtmlToPdf _pdfRenderer;

        public MedicinePdfService()
        {
            _pdfRenderer = new HtmlToPdf();
        }

        public void GenerateFor(FullMedicineDTO medicine)
        {
            var medicineTemplateHtmlText = File.ReadAllText("medicine-template.html");
            var template = Template.Parse(medicineTemplateHtmlText);
            var medicineHtmlText = template.Render(new { Medicine = medicine });

            _pdfRenderer.RenderHtmlAsPdf(medicineHtmlText).SaveAs(GetPdfName(medicine));
        }

        public FileStream GetPdfStreamFor(FullMedicineDTO medicine)
        {
            var pdfName = GetPdfName(medicine);
            if (!File.Exists(pdfName))
            {
                GenerateFor(medicine);
            }

            return new FileStream(pdfName, FileMode.Open, FileAccess.Read);
        }

        public void DeleteFor(FullMedicineDTO medicine)
        {
            var pdfName = GetPdfName(medicine);
            if (File.Exists(pdfName))
            {
                File.Delete(pdfName);
            }
        }

        private string GetPdfName(FullMedicineDTO medicine)
        {
            return $"{medicine.Medicine.UniqueId}.pdf";
        }
    }
}