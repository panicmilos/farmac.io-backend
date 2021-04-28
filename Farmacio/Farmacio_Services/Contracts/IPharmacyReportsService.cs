using System;
using System.Collections.Generic;
using Farmacio_Models.DTO;

namespace Farmacio_Services.Contracts
{
    public interface IPharmacyReportsService
    {
        IEnumerable<PharmacyReportRecordDTO> GenerateExaminationsReportFor(Guid pharmacyId, TimePeriodDTO timePeriod);
        IEnumerable<PharmacyReportRecordDTO> GenerateMedicineConsumptionReportFor(Guid pharmacyId, TimePeriodDTO timePeriod);
    }
}