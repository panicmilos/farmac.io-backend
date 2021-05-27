using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;

namespace Farmacio_Services.Implementation
{
    public class GradeService : CrudService<Grade>, IGradeService
    {
        public GradeService(IRepository<Grade> repository):
            base(repository)
        {

        }
    }
}
