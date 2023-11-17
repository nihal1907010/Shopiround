using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Shopiround.Data;
using Shopiround.Models;
using System;
using System.Threading.Tasks;

namespace Shopiround.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public ApplicationDbContext applicationDbContext;
        public ChatHub(ApplicationDbContext applicationDbContext) { 
            this.applicationDbContext = applicationDbContext;
        }
        public override Task OnConnectedAsync()
        {
            var user1 = Context.ConnectionId;
            var user2 = Context.UserIdentifier;
            return base.OnConnectedAsync();
        }
        public async Task SendMessage(string senderId, string receiverId, string productId, string messageTxt)
        {
            applicationDbContext.Message.Add(new Message
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                ProductId = int.Parse(productId),
                MessageText = messageTxt,
                SendTime = DateTime.Now
            });
            applicationDbContext.SaveChanges();
            //await Clients.All.SendAsync("ReceiveMessage", senderId, messageTxt);
            await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, messageTxt);
        }
    }
}
