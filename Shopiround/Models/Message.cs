using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopiround.Models
{
    public class Message
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string MessageText {  get; set; }
        public DateTime SendTime { get; set; }
        public DateTime? SeenTime { get; set; }

    }
}
