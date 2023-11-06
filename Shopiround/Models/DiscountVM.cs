using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopiround.Models.Models
{
    public class DiscountVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public float DiscountAmount { get; set; }
        public float DiscountParcentage { get; set; }
    }
}
