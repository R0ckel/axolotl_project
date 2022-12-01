using System.ComponentModel.DataAnnotations;
using AxolotlProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AxolotlProject.Areas.Identity.Pages.Account.Manage
{
    public class SettingsModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<ChangePasswordModel> _logger;

        public SettingsModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<ChangePasswordModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }
        public string Login { get; set; }

        public class InputModel
        {
            [Display(Name = "Your login")]
            public string NewLogin { get; set; }

            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [Display(Name = "Your password")]
            public string NewPassword { get; set; }

            [Display(Name = "Repeat your password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var login = user.Login ?? "";
            Login = login;

            Input = new InputModel
            {
                NewLogin = login
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            var hasPassword = await _userManager.HasPasswordAsync(user);
            if (!hasPassword)
                return RedirectToPage("./SetPassword");

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            if(Input.NewPassword is not null) {
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword, Input.NewPassword);
            }

            if(Input.NewLogin is not null && Input.NewLogin != user.Login) 
                user.Login = Input.NewLogin;

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their data successfully.");

            return RedirectToPage();
        }
    }
}
