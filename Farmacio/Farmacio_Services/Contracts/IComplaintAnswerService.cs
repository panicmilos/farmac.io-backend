using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IComplaintAnswerService : ICrudService<ComplaintAnswer>
    {
        IEnumerable<ComplaintAnswer> ReadBy(Guid writerId);
    }
}