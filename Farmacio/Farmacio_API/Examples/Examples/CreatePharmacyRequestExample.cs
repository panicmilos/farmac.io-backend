using Farmacio_API.Contracts.Requests.Addresses;
using Farmacio_API.Contracts.Requests.Pharmacies;
using Swashbuckle.AspNetCore.Filters;

namespace Farmacio_API.Examples.Examples
{
    public class CreatePharmacyRequestExample : IExamplesProvider<CreatePharmacyRequest>
    {
        public CreatePharmacyRequest GetExamples()
        {
            return new CreatePharmacyRequest
            {
                Name = "Apoteka Jankovic",
                Description = "Poverenje, sigurnost i dostupnost su, već skoro 30 godina, glavna obeležja Apotekarske ustanove “Janković”. Ljubav i podrška koju svakodnevno dobijamo od vernih klijenata omogućila je da postanemo jedna od najpoznatijih i najposećenijih apoteka.",
                Address = new CreateAddressRequest
                {
                    State = "Srbija",
                    City = "Novi Sad",
                    StreetName = "Gunduliceva",
                    StreetNumber = "26",
                    Lat = 45.263910F,
                    Lng = 19.847410F
                }
            };
        }
    }
}