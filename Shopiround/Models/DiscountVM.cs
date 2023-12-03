using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopiround.Models
{
    public class DiscountVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public String ImageURL { get; set; }
        public float Price { get; set; }
        public float DiscountAmount { get; set; } = 0;
        public float DiscountParcentage { get; set; } = 0;
        
        public int TotalDays { get; set; }
        
        public bool TodayDiscount { get; set; }
    }
}
