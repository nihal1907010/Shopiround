using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopiround.Models
{
    public  class Product
    {
        [Key]
        public int Id { get; set; }
        public String Name { get; set; }
        public String? Description { get; set; }
        public int Quantity { get; set; }
        public float Price { get; set; }
        public String Category { get; set; }
        public float DiscountPercentage { get; set; }
        public float DiscountAmount { get; set; }
        public String ImageURL { get; set; }
        [ForeignKey("Shop")]
        public int ShopId { get; set; }
        public Shop Shop { get; set; }
        public List<Review> Reviews { get; set; }
        public List<Question> Questions { get; set; }
    }
}
