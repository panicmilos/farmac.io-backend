using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Implementation.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class ComplaintService<T> : CrudService<T>, IComplaintService<T> where T : Complaint
    {
        public ComplaintService(IRepository<T> repository) :
            base(repository)
        {
        }

        public IEnumerable<T> ReadBy(Guid writerId)
        {
            return Read().Where(complaint => complaint.WriterId == writerId).ToList();
        }

        public IEnumerable<T> ReadPagesToComplaints(PageDTO page, bool showAll)
        {
            if (showAll)
            {
                return ReadAllPagesTo(page);
            }

            var notAnsweredComplaints = Read().ToList().Where(complaint => !complaint.Answers.Any());
            return PaginationUtils<T>.PagesTo(notAnsweredComplaints, page);
        }

        public IEnumerable<T> ReadPagesToOfComplaintsBy(Guid writerId, PageDTO page)
        {
            return PaginationUtils<T>.PagesTo(ReadBy(writerId), page);
        }
    }
}