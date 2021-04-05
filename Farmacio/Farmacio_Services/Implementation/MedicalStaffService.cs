using System;
using System.Collections.Generic;
using System.Linq;
using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;


namespace Farmacio_Services.Implementation
{
    public class MedicalStaffService : AccountService, IMedicalStaffService
    {
        private readonly IAppointmentService _appointmentService;

        public MedicalStaffService(IEmailVerificationService emailVerificationService, IAppointmentService appointmentService,
            IRepository<Account> repository)
            : base(emailVerificationService, repository)
        {
            _appointmentService = appointmentService;
        }
    }
}
