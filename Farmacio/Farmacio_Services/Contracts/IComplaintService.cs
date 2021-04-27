using Farmacio_Models.Domain;

namespace Farmacio_Services.Contracts
{
    public interface IComplaintService<T> : ICrudService<T> where T : Complaint
    {
    }
}