using ChatApp.Data;
using ChatApp.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ChatApp.Hubs
{
    public class ChatHub: Hub
    {
      
        public async Task SendMessage(string message, string userId, string receiverId, ChatDbContext _context)
        {
            var UserMessage = new Message
            {
                message = message,
                receiverId = receiverId,
                senderId = userId,               

            };
           
            _context.Add(UserMessage);
            _context.SaveChanges();
            string jsonMessage = JsonSerializer.Serialize(UserMessage);
            await Clients.All.SendAsync("ReceiveMessage", jsonMessage);
            await Clients.User("1234").SendAsync("", jsonMessage);
        }

        public async Task GetMessages(string recId, string userId, ChatDbContext _context)
        {
            var messages = _context.messages.Where(u => (u.receiverId == recId && u.senderId == userId) || (u.senderId == recId && u.receiverId == userId)).ToList();
            string jsonMessage = JsonSerializer.Serialize(messages);
            await Clients.All.SendAsync("ReceiveAllMessage", jsonMessage);

        }


    }
}
