using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopiround.Models.Models
{
    public class Shop
    {
        [Key]
        public int ShopId { get; set; }
        [DisplayName("Shop Name")]
        public string ShopName { get; set; }
        public string ShopPhoneNo { get; set; }
        public string OwnerName { get; set; }
        public string OwnerPhoneNo { get; set; }
        public string Division {  get; set; }
        public string District { get; set; }
        public string Upazila { get; set; }
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        //public TimeOnly OpenningTime { get; set; }
        //public TimeOnly ClosingTime { get; set; }
        //public string OpenDays { get; set; }
        [DisplayName("Accept Online Orders?")]
        public bool AcceptOnlineOrders { get; set; }

    }
}
