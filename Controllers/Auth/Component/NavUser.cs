using ChatApp.Data;
using ChatApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Controllers.Auth.Component
{
    
    public class NavUser : ViewComponent
    {
        private readonly UserManager<ChatUser> _userManager;
        private readonly ChatDbContext _context;

        public NavUser(UserManager<ChatUser> userManager, ChatDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var users = _userManager.Users.Include(x => x.messages).AsNoTracking().Select(a => new ChatUser
            {
                UserName = a.UserName,
                Id = a.Id,
                messages = _context.messages.Where(u =>  (u.receiverId == a.Id && u.senderId == UserClaimsPrincipal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value) || (u.senderId == a.Id && u.receiverId == UserClaimsPrincipal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value)).OrderBy(x=>x.Id).AsNoTracking().ToList(),
                imagePath = a.imagePath
            }).ToList();
            return View(users);
        }
    }
}
