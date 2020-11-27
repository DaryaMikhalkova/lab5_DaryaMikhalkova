using Microsoft.AspNetCore.Identity;
using SewingStudio.Models;
using System.Threading.Tasks;

namespace CustomIdentityApp
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "1234admin";
            if (await roleManager.FindByNameAsync("Администратор") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Администратор"));
            }
            if (await roleManager.FindByNameAsync("Покупатель") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Покупатель"));
            }
            if (await roleManager.FindByNameAsync("Работник") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Работник"));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User { Email = adminEmail, UserName = adminEmail };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Администратор");
                }
            }
        }
    }
}