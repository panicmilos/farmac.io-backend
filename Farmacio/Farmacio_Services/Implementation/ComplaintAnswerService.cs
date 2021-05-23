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
        private IComplaintAnswerRepository complaintAnswerRepository { get => _repository as IComplaintAnswerRepository; }
        private readonly IComplaintService<Complaint> _complaintService;
        private readonly ISystemAdminService _systemAdminService;

        public ComplaintAnswerService(
            IComplaintService<Complaint> complaintService,
            ISystemAdminService systemAdminService,
            IComplaintAnswerRepository repository
        ) :
            base(repository)
        {
            _complaintService = complaintService;
            _systemAdminService = systemAdminService;
        }

        public override ComplaintAnswer Create(ComplaintAnswer answer)
        {
            using var transaction = _repository.OpenTransaction();
            try
            {
                _complaintService.TryToRead(answer.ComplaintId);
                if (_systemAdminService.ReadByUserId(answer.WriterId) == null)
                {
                    throw new MissingEntityException("Given system admin doesn't exist in the system.");
                }

                if (complaintAnswerRepository.ReadAnswersFor(answer.ComplaintId).Any())
                {
                    throw new BadLogicException("Only one system admin can give an answer to complaint.");
                }

                var createdAnswer = base.Create(answer);
                transaction.Commit();

                return createdAnswer;
            }
            catch (InvalidOperationException)
            {
                transaction.Rollback();
                throw new BadLogicException("Something bad happend. Please try again.");
            }
        }

        public IEnumerable<ComplaintAnswer> ReadBy(Guid writerId)
        {
            return Read().Where(answer => answer.WriterId == writerId).ToList();
        }

        public IEnumerable<ComplaintAnswer> ReadPagesToOfAnswersBy(Guid writerId, PageDTO page)
        {
            return PaginationUtils<ComplaintAnswer>.PagesTo(ReadBy(writerId), page);
        }
    }
}