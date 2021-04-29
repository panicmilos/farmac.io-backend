using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;

namespace Farmacio_Services.Implementation
{
    public class ComplaintService<T> : CrudService<T>, IComplaintService<T> where T : Complaint
    {
        public ComplaintService(IRepository<T> repository) :
            base(repository)
        {
        }
    }
}