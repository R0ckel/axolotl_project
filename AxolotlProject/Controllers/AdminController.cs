using AxolotlProject.Data;
using AxolotlProject.Models;
using AxolotlProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.X86;

namespace AxolotlProject.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<UserPostController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public AdminController(ILogger<UserPostController> logger, ApplicationDbContext context, UserManager<User> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index(string? login = null)
        {
            var vm = new AdminViewModel();
            vm.searchedUsers = _context.Users
                .Where(u=>u.Login==null || login==null || u.Login.Contains(login))
                .Where(u=>u.IsBanned == false);
            vm.bannedUsers = _context.Users.Where(u => u.IsBanned);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Ban(string id)
        {
            var toBan = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            if (toBan != null)
            {
                toBan.IsBanned = true;
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UnBan(string id)
        {
            var toBan = _context.Users.Where(u => u.Id == id).FirstOrDefault();
            if (toBan != null)
            {
                toBan.IsBanned = false;
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
