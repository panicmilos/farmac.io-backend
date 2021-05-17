using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using System;
using System.Collections.Generic;

namespace Farmacio_Services.Contracts
{
    public interface IComplaintAnswerService : ICrudService<ComplaintAnswer>
    {
        IEnumerable<ComplaintAnswer> ReadBy(Guid writerId);

        IEnumerable<ComplaintAnswer> ReadPagesToOfAnswersBy(Guid writerId, PageDTO page);
    }
}