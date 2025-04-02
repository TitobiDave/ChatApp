using ChatApp.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatApp.Data
{
    public static class IdentitySeedData
    {
        public async static void SeedData(IApplicationBuilder _app)
        {
            var userManager = _app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<ChatUser>>();
            var context = _app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ChatDbContext>();
            if (!userManager.Users.Any())
            {
                var user = new List<ChatUser>()
                {
                    new ChatUser
                    {
                         UserId = Guid.NewGuid().ToString(),
                         UserName = "Tito",
                         Email = "titobioluwole@gmail.com",
                         PhoneNumber = "1234567890",
                    },
                     new ChatUser
                     {
                    UserId = Guid.NewGuid().ToString(),
                    UserName = "Fiyin",
                    Email = "Fiyin.com",
                    PhoneNumber = "1234567890",
                     },
                     new ChatUser
                {
                    UserId = Guid.NewGuid().ToString(),
                    UserName = "Mike",
                    Email = "JohnDoe@gmail.com",
                    PhoneNumber = "1234567890",
                }

                };
                foreach(var item in user)
                {
                    item.PasswordHash = userManager.PasswordHasher.HashPassword(item, "Titobi12345_");
                }
                context.Users.AddRange(user);
                context.SaveChanges();  

            }
        }
    }
}
