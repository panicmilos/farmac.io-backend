using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farmacio_Services.Contracts
{
    public interface IMedicineGradeService: IGradeService
    {
        IEnumerable<Medicine> ReadThatPatientCanRate(Guid patientId);
    }
}
