namespace Shopiround.Models
{
    public class OnlineOrderVM
    {
        public UserProfile User { get; set; }
        public int TotalOrders { get; set; }
        public int TotalPrice { get; set; }
    }
}
