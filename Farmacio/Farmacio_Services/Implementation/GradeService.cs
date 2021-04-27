using Farmacio_Models.Domain;
using Farmacio_Repositories.Contracts;
using Farmacio_Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

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
