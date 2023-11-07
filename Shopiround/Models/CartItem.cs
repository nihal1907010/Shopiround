using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopiround.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }


        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
