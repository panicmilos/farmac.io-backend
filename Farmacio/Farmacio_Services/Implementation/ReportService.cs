using System;
using System.Collections.Generic;
using System.Linq;
using EmailService.Constracts;
using EmailService.Models;
using Farmacio_Models.Domain;
using Farmacio_Models.DTO;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using Farmacio_Services.Exceptions;
using Farmacio_Services.Implementation.Utils;
using GlobalExceptionHandler.Exceptions;


namespace Farmacio_Services.Implementation
{
    public class ReportService : CrudService<Report>, IReportService
    {
        public ReportService(IRepository<Report> repository) : base(repository)
        {

        }
    }
}
