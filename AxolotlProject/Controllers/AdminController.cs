using AxolotlProject.Data;
using AxolotlProject.Models;
using AxolotlProject.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.X86;

namespace AxolotlProject.Controllers
{
    [Authorize(Roles = "Administrator,CookingModerator,GamingModerator,ProgrammingModerator,LiteratureModerator,DiscoveryModerator,EngineeringModerator,PsychologyModerator,EconomyModerator,ScienceModerator,ShoppingModerator,AnimalsModerator,PlantsModerator,TravellingModerator,HistoryModerator,MemesModerator,SpeakingModerator")]
    public class AdminController : Controller
    {
        private readonly ILogger<UserPostController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private static PostCategory _currentCategory { get; set; }

        public AdminController(ILogger<UserPostController> logger, ApplicationDbContext context, UserManager<User> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string? login = null)
        {
            ViewBag.CurrentUser = await _userManager.GetUserAsync(HttpContext.User);
            if(_context.IsUserBanned(ViewBag.CurrentUser?.Id, PostCategory.All)) 
                return Forbid();
            string role = (await _userManager.GetRolesAsync(ViewBag.currentUser))[0];
            ViewBag.CurrentUserRole = role;
            ViewBag.CurrentCategory = _currentCategory = role.Equals("Administrator") ? PostCategory.All : System.Enum.Parse<PostCategory>(role.Remove(role.Length - 9, 9));
            var vm = new AdminViewModel();
            foreach (var ban in _context.Bans)
                Console.WriteLine(ban.Category.ToString() + ' ' + _currentCategory.ToString());
            vm.bannedUsers = _context.Bans
                            .Where(b => b.Category.Equals(_currentCategory))
                            .Select(b => b.UserId)
                            .Distinct()
                            .Select(id => _context.Users.FirstOrDefault(u => u.Id.Equals(id)));
            vm.searchedUsers = _context.Users
                .Where(u => (u.Login != null && login != null && u.Login.Contains(login)));
            var users = _context.Users.ToList();
            foreach (var bUser in vm.bannedUsers)
                users.Remove(bUser);
            vm.Users = users;
            ViewBag.BanDates = vm.bannedUsers.ToDictionary(u => u.Id, u => _context.Bans.FirstOrDefault(b => b.UserId.Equals(u.Id) && b.Category.Equals(_currentCategory))?.BanDate);
            ViewBag.BannersNames = vm.bannedUsers.ToDictionary(u => u.Id, u => _context.Users.FirstOrDefault(a => a.Id == _context.Bans.FirstOrDefault(b => b.UserId.Equals(u.Id) && b.Category.Equals(_currentCategory)).BannerId).Login);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Ban(string id)
        {
            var banner = await _userManager.GetUserAsync(HttpContext.User);
            if(_context.IsUserBanned(banner?.Id, PostCategory.All)) 
                return Forbid();
            var bannable = _context.Users.Any(u => u.Id == id && 
                !_context.Bans.Any(b => b.UserId.Equals(id) && b.Category.Equals(_currentCategory)));
            if (bannable)
                _context.Bans?.Add(new Ban(id, banner.Id, _currentCategory));
            await _context.SaveChangesAsync();
            Console.WriteLine(_currentCategory.ToString());
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UnBan(string id)
        {
            var unbanner = await _userManager.GetUserAsync(HttpContext.User);
            if(_context.IsUserBanned(unbanner?.Id, PostCategory.All)) 
                return Forbid();
            var unbannable = _context.Bans.FirstOrDefault(u => u.UserId.Equals(id) && u.Category.Equals(_currentCategory));
            if (unbannable is not null && (_currentCategory.Equals(unbannable.Category) /*|| _currentCategory.Equals(PostCategory.All) */))
                _context.Bans?.Remove(_context.Bans?.FirstOrDefault(b => b.UserId.Equals(id) && b.Category.Equals(_currentCategory)));
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
