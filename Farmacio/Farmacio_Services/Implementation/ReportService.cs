using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;


namespace Farmacio_Services.Implementation
{
    public class ReportService : CrudService<Report>, IReportService
    {
        public ReportService(IRepository<Report> repository) : base(repository)
        {

        }
    }
}
