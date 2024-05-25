using System;
using Microsoft.AspNetCore.SignalR;

namespace WebApiDemo.Controllers
{
    public class ChatHub : Hub
    {
        public async Task ReceiveMessage(Message message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}

