using System.Collections.Generic;
using Farmacio_Models.Domain;

namespace Farmacio_API.Contracts.Responses.Dermatologists
{
    public class DermatologistWithWorkPlacesResponse
    {
        public Account DermatologistAccount { get; set; }
        public IEnumerable<DermatologistWorkPlace> WorkPlaces { get; set; }
    }
}