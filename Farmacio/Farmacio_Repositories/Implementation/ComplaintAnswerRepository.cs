using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Repositories.Implementation
{
    public class ComplaintAnswerRepository : Repository<ComplaintAnswer>, IComplaintAnswerRepository
    {
        public ComplaintAnswerRepository(DatabaseContext context) :
            base(context)
        {
        }

        public IEnumerable<ComplaintAnswer> ReadAnswersFor(Guid complaintId)
        {
            return _entities.FromSqlRaw($"SELECT * FROM ComplaintAnswers WHERE ComplaintId = \"{complaintId}\" FOR UPDATE;").ToList();
        }
    }
}