// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AxolotlProject.Models;
using AxolotlProject.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AxolotlProject.Areas.Identity.Pages.Account.Manage
{
    public class ProfileModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;

        public ProfileModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public string Username { get; set; }
        public string Description { get; set; }
        public bool Editable { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Display(Name = "Description")]
            public string Description { get; set; }
        }

        private async Task LoadAsync(User user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Description = user.Description
            };
        }

        public async Task<IActionResult> OnGetAsync(string? id = null)
        {
            User user;
            if(id is null)
                user = await _userManager.GetUserAsync(User);
            else
                user = _context.Users.FirstOrDefault(u => u.Id.Equals(id));
            Username = user.UserName;
            Description = user.Description;

            var currentUser = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

            Editable = currentUser.Id.Equals(user.Id);

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? id = null)
        {
            User user;
            if(id is null)
                user = await _userManager.GetUserAsync(User);
            else
                user = _context.Users.FirstOrDefault(u => u.Id.Equals(id));
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var currentUser = await _userManager.GetUserAsync(User);
            Editable = currentUser.Id.Equals(user.Id);

            if (!ModelState.IsValid || !Editable)
            {
                await LoadAsync(user);
                return Page();
            }

            if (Input.Description != user.Description)
            {
                user.Description = Input.Description;
            }

            await _userManager.UpdateAsync(user);

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";

            return RedirectToPage();
        }
    }
}
