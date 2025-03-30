using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace chatapp.Hubs
{
    
    public class ChatHub : Hub
    {
        public async Task SendMessage(Guid SenderId,Guid ReceiverId, string message)
        {
            
            await Clients.All.SendAsync("ReceiveMessage", SenderId,ReceiverId, message);
        }

        // public override async Task OnConnectedAsync()
        // {
            
        //     var userId = Context.UserIdentifier;
        //     Console.WriteLine($"User {userId} connected");
        //     await base.OnConnectedAsync();
        // }
    }
}