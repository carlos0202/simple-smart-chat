using Microsoft.AspNetCore.SignalR;
using Simple.Smart.Chat.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple.Smart.Chat.App.Infrastructure.Hubs
{
    public class ChatRoomHub : Hub
    {
        public async Task SendMessage(ChatMessage chatMessage)
        {
            await Clients.All.SendAsync("receiveMessage", chatMessage);
        }
    }
}