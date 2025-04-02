using ChatApp.Models;
using ChatApp.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.Controllers.Auth
{
    public class AccountController : Controller
    {
        private readonly UserManager<ChatUser> _userManager;
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<ChatUser> _signInManager;
        public AccountController(UserManager<ChatUser> userManager, ILogger<AccountController> logger, SignInManager<ChatUser> signInManager)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel login)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x=>x.Email == login.Email);
            var claims = User.Claims;
            if(user == null)
            {
                ModelState.AddModelError("Invalid Login", "Login Failed");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user, login.Password, true, false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in");
                return Redirect("/Home/Index");
            }

            
            return View(new { errorMessage = "Invalid Login Attempt" });
        }
    }
}
