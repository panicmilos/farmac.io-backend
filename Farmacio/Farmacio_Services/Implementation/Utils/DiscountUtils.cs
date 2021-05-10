namespace Farmacio_Services.Implementation.Utils
{
    public static class DiscountUtils
    {
        public static float ApplyDiscount(float originalPrice, int discount)
        {
            return (100 - discount) * originalPrice / 100;
        }
    }
}