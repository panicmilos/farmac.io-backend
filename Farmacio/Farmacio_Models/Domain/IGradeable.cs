using System.Collections.Generic;

namespace Farmacio_Models.Domain
{
    public interface IGradeable
    {
        int AverageGrade { get; set; }
        List<Grade> Grades { get; set; }
    }
}
