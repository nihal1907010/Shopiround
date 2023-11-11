using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopiround.Models
{
    public class PurchaseItem
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public bool Online { get; set; }
        public bool Reserve { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
