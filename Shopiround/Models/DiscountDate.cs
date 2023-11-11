using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopiround.Models
{
    public class DiscountDate
    {
        public int Id { get; set; }
        public int productId { get; set; }
        public DateTime discountEndDate { get; set; }
        public bool TodayDiscount { get; set; }

    }
}
