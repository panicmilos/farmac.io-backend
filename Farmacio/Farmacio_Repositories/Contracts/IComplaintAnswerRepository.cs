using Farmacio_Models.Domain;
using System;
using System.Collections.Generic;

namespace Farmacio_Repositories.Contracts
{
    public interface IComplaintAnswerRepository : IRepository<ComplaintAnswer>
    {
        IEnumerable<ComplaintAnswer> ReadAnswersFor(Guid complaintId);
    }
}