using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopiround.Models
{
    public class Notification
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
    }
}
