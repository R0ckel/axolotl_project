using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using AxolotlProject.Models;

namespace AxolotlProject.Seeds;

public static class UserSeed
{
    public static async Task SeedUserAsync(UserManager<User> userManager)
    {
        var defaultUser = new User
        {
            Login = "user",
            UserName = "user@gmail.com",
            Email = "user@gmail.com",
            EmailConfirmed = true
        };
        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
                await userManager.CreateAsync(defaultUser, "Pass_1234");
        }
        Console.WriteLine("seeded user");
    }
    public static async Task SeedAdministratorAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        var defaultUser = new User
        {
            Login = "admin",
            UserName = "admin@gmail.com",
            Email = "admin@gmail.com",
            EmailConfirmed = true
        };
        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "Pass_1234");
                await userManager.AddToRoleAsync(defaultUser, "Administrator");
            }
        }
        Console.WriteLine("seeded admin");
    }
    public static async Task SeedModeratorAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        var defaultUser = new User
        {
            Login = "moderator",
            UserName = "moderator@gmail.com",
            Email = "moderator@gmail.com",
            EmailConfirmed = true
        };
        if (userManager.Users.All(u => u.Id != defaultUser.Id))
        {
            var user = await userManager.FindByEmailAsync(defaultUser.Email);
            if (user == null)
            {
                await userManager.CreateAsync(defaultUser, "Pass_1234");
                await userManager.AddToRoleAsync(defaultUser, "ProgrammingModerator");
            }
        }
        Console.WriteLine("seeded mod");
    }
}
