using Microsoft.AspNetCore.Identity;

namespace ChatApp.Models
{
    public class ChatUser:IdentityUser
    {
        public string UserId { get; set; }

        public string imagePath {  get; set; }
        public List<Message> messages { get; set; }
    }
}
