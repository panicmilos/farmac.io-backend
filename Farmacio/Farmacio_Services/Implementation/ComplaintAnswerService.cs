using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Implementation.Utils;
using GlobalExceptionHandler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Farmacio_Services.Implementation
{
    public class ComplaintAnswerService : CrudService<ComplaintAnswer>, IComplaintAnswerService
    {
        private readonly IComplaintService<Complaint> _complaintService;
        private readonly ISystemAdminService _systemAdminService;

        public ComplaintAnswerService(
            IComplaintService<Complaint> complaintService,
            ISystemAdminService systemAdminService,
            IRepository<ComplaintAnswer> repository
        ) :
            base(repository)
        {
            _complaintService = complaintService;
            _systemAdminService = systemAdminService;
        }

        public override ComplaintAnswer Create(ComplaintAnswer answer)
        {
            _complaintService.TryToRead(answer.ComplaintId);
            if (_systemAdminService.ReadByUserId(answer.WriterId) == null)
            {
                throw new MissingEntityException("Given system admin doesn't exist in the system.");
            }

            if (HasSystemAdminAnsweredComplaint(answer.WriterId, answer.ComplaintId))
            {
                throw new BadLogicException("System admin already answered given complaint.");
            }

            return base.Create(answer);
        }

        public IEnumerable<ComplaintAnswer> ReadBy(Guid writerId)
        {
            return Read().Where(answer => answer.WriterId == writerId).ToList();
        }

        public IEnumerable<ComplaintAnswer> ReadPagesToOfAnswersBy(Guid writerId, PageDTO page)
        {
            return PaginationUtils<ComplaintAnswer>.PageTo(ReadBy(writerId), page);
        }

        private bool HasSystemAdminAnsweredComplaint(Guid systemAdminId, Guid complaintId)
        {
            return Read().FirstOrDefault(answer => answer.WriterId == systemAdminId &&
                                                   answer.ComplaintId == complaintId) != null;
        }
    }
}