using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using GlobalExceptionHandler.Exceptions;
using System;
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

        private bool HasSystemAdminAnsweredComplaint(Guid systemAdminId, Guid complaintId)
        {
            return Read().FirstOrDefault(answer => answer.WriterId == systemAdminId &&
                                                   answer.ComplaintId == complaintId) != null;
        }
    }
}