using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopiround.Models
{
    public class Shop
    {
        [Key]
        public int ShopId { get; set; }
        [DisplayName("Shop Name")]
        public string ShopName { get; set; }
        [DisplayName("Shop Phone No.")]
        public string ShopPhoneNo { get; set; }
        [DisplayName("Owner Name")]
        public string OwnerName { get; set; }
        public string Division {  get; set; }
        public string District { get; set; }
        public string Upazila { get; set; }
        public string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string OpeningTime { get; set; }
        public string ClosingTime { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }

        [DisplayName("Accept Online Orders?")]
        public bool AcceptOnlineOrders { get; set; }
        public string ImageURL { get; set; }  
        List<Product> Products { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; }
        public ApplicationUser applicationUser { get; set; }

    }
}
