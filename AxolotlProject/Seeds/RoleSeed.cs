using Microsoft.AspNetCore.Identity;
using AxolotlProject.Models;

namespace AxolotlProject.Seeds;

public static class RoleSeed
{
    public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
    {
        await roleManager.CreateAsync(new IdentityRole("Administrator"));
        foreach (string role in System.Enum.GetNames(typeof(PostCategory)))
            await roleManager.CreateAsync(new IdentityRole(role + "Moderator"));
        Console.WriteLine("seeded roles");
    }

    public static string AdminAndModersList => "Administrator," + String.Join(',', System.Enum.GetNames(typeof(PostCategory)).Select(m => m + "Moderator"));
}
