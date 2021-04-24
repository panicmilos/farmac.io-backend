using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;

namespace Farmacio_Services.Implementation
{
    public class ERecipeService : CrudService<ERecipe>, IERecipeService
    {
        public ERecipeService(IRepository<ERecipe> repository) : base(repository)
        {

        }


    }
}
