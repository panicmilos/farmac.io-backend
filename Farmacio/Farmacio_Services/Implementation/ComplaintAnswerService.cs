using EmailService.Constracts;
using EmailService.Models;
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
        private IComplaintAnswerRepository ComplaintAnswerRepository => _repository as IComplaintAnswerRepository;
        private readonly IComplaintService<Complaint> _complaintService;
        private readonly ISystemAdminService _systemAdminService;
        private readonly IPatientService _patientService;
        private readonly IEmailDispatcher _emailDispatcher;
        private readonly ITemplatesProvider _templatesProvider;

        public ComplaintAnswerService(
            IComplaintService<Complaint> complaintService,
            ISystemAdminService systemAdminService,
            IComplaintAnswerRepository repository,
            IPatientService patientService,
            IEmailDispatcher emailDispatcher,
            ITemplatesProvider templatesProvider
        ) :
            base(repository)
        {
            _complaintService = complaintService;
            _systemAdminService = systemAdminService;
            _patientService = patientService;
            _emailDispatcher = emailDispatcher;
            _templatesProvider = templatesProvider;
        }

        public override ComplaintAnswer Create(ComplaintAnswer answer)
        {
            using var transaction = _repository.OpenTransaction();
            try
            {
                var complaint = _complaintService.TryToRead(answer.ComplaintId);
                var writterAccount = _systemAdminService.ReadByUserId(answer.WriterId);
                if (writterAccount == null)
                {
                    throw new MissingEntityException("Given system admin doesn't exist in the system.");
                }
                var writter = writterAccount.User as SystemAdmin;

                if (ComplaintAnswerRepository.ReadAnswersFor(answer.ComplaintId).Any())
                {
                    throw new BadLogicException("Only one system admin can give an answer to complaint.");
                }

                var createdAnswer = base.Create(answer);

                var email = _templatesProvider.FromTemplate<Email>("ComplaintAnswer", new
                {
                    To = _patientService.ReadByUserId(complaint.WriterId).Email,
                    Patient = complaint.Writer.FirstName,
                    Admin = $"{writter.FirstName} {writter.LastName}",
                    About = complaint.For(),
                    Answer = createdAnswer.Text
                });
                _emailDispatcher.Dispatch(email);

                transaction.Commit();

                return createdAnswer;
            }
            catch (InvalidOperationException)
            {
                transaction.Rollback();
                throw new BadLogicException("Something bad happened. Please try again.");
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