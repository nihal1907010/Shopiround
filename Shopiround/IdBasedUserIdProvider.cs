using Microsoft.AspNetCore.SignalR;

namespace Shopiround
{
    public class IdBasedUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User.Identity.Name;
        }
    }
}
