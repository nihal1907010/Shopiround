using System.Collections.Generic;

namespace Shopiround.Models
{
    public class ShopProfile
    {
        public Shop shop { get; set; }
        public  List<Product> products { get; set; }
    }
}
