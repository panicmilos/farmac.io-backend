namespace Farmacio_Models.Domain
{
    public class Address
    {
        public string StreetName { get; set; }
        public string StreetNumber { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
    }
}
