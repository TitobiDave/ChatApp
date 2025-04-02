using ChatApp.Data;
using ChatApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ChatApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ChatDbContext _context;
        private readonly UserManager<ChatUser> _userManager;
        private readonly string _imageDirectory = "wwwroot/uploads";
        public HomeController(ILogger<HomeController> logger, ChatDbContext context, UserManager<ChatUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Update([FromQuery] string userId)
        {
            ViewBag.UserId = userId;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Update(IFormFile file, string userId)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            string fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(_imageDirectory, fileName);

            // Ensure directory exists
            if (!Directory.Exists(_imageDirectory))
                Directory.CreateDirectory(_imageDirectory);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                return BadRequest("User no exists");
            }
            user.imagePath = "/uploads/" + fileName;
            await _context.SaveChangesAsync();

            return Ok("Successful");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
