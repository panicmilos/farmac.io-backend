using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IComplaintService<T> : ICrudService<T> where T : Complaint
    {
        IEnumerable<T> ReadBy(Guid writerId);
    }
}