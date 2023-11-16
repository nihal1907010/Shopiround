using System.ComponentModel.DataAnnotations.Schema;

namespace Shopiround.Models
{
    public class LastMessage
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver {  get; set; }
        [ForeignKey("Message")]
        public int MessageId { get; set; }
        public Message Message { get; set; }
    }
}
