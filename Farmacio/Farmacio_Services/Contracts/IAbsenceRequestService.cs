using System;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IAbsenceRequestService : ICrudService<AbsenceRequest>
    {
        IEnumerable<AbsenceRequest> ReadFor(Guid pharmacyId);
        IEnumerable<AbsenceRequest> CreateAbsenceRequest(AbsenceRequestDTO absenceRequestDto); 
    }
}
