using System.ComponentModel.DataAnnotations.Schema;

namespace Shopiround.Models.Statistics
{
    public class ProductCount
    {
        public int Id { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Count { get; set; }
    }
}
