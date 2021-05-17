using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IComplaintService<T> : ICrudService<T> where T : Complaint
    {
        IEnumerable<T> ReadBy(Guid writerId);

        IEnumerable<T> ReadPagesToOfComplaintsBy(Guid writerId, PageDTO page);
    }
}