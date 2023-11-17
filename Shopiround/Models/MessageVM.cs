using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Collections.Generic;

namespace Shopiround.Models
{
    public class MessageVM
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public Product Product { get; set; }
        public List<Message> OldMessages { get; set; }
    }
}
