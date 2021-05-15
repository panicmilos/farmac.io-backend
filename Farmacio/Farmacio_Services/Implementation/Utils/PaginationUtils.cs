using System.Collections.Generic;
using System.Linq;
using Farmacio_Models.DTO;

namespace Farmacio_Services.Implementation.Utils
{
    public class PaginationUtils<T>
    {
        public static IEnumerable<T> Page(IEnumerable<T> enumerable, PageDTO pageDto)
        {
            var numberOfEntitiesToSkip = (pageDto.Number - 1) * pageDto.Size;
            return enumerable
                .Skip(numberOfEntitiesToSkip)
                .Take(pageDto.Size);
        }

        public static IEnumerable<T> PageTo(IEnumerable<T> enumerable, PageDTO pageDto)
        {
            var numberOfEntitiesToTake = pageDto.Number * pageDto.Size;
            return enumerable.Take(numberOfEntitiesToTake);
        }
    }
}