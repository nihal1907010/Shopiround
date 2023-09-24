using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopiround.Models.Models
{
    internal class Product
    {
        [Key]
        public int Id { get; set; }
        public int Name { get; set; }
        public int Description { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public String Category { get; set; }
        public float DiscountPercentage { get; set; }
        public int DiscountAmount { get; set; }

    }
}
