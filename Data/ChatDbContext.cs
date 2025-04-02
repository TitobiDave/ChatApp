using ChatApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Data
{
    public class ChatDbContext: IdentityDbContext<ChatUser>
    {
        public ChatDbContext(DbContextOptions<ChatDbContext> options)
            :base(options)
        {
                
        }



        public DbSet<ChatUser> chatUsers { get; set; }

        public DbSet<Message> messages { get; set; }    
         
    }
}
