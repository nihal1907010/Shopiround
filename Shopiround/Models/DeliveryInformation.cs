namespace Shopiround.Models
{
    public class DeliveryInformation
    {
        public int Id {  get; set; }
        public int cartItemId { get; set; }
        public CartItem CartItem { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
}
