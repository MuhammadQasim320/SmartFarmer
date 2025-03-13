using Microsoft.AspNetCore.Identity;
using SmartFarmer.Domain.Model;

namespace SmartFarmer.API.Helper
{
    public class RoleInitializer
    {
        public static async Task Initialize(RoleManager<IdentityRole> roleManager, UserManager<Domain.Model.ApplicationUser> userManager)
        {
            if (!await roleManager.RoleExistsAsync("SuperAdmin"))
            {
                var role = new IdentityRole("SuperAdmin");
                await roleManager.CreateAsync(role);
            }
            if (!await roleManager.RoleExistsAsync("MasterAdmin"))
            {
                var role = new IdentityRole("MasterAdmin");
                await roleManager.CreateAsync(role);
            }
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var role = new IdentityRole("Admin");
                await roleManager.CreateAsync(role);
            }
            if (!await roleManager.RoleExistsAsync("Portal"))
            {
                var role = new IdentityRole("Portal");
                await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync("Operator"))
            {
                var role = new IdentityRole("Operator");
                await roleManager.CreateAsync(role);
            }

            if (!await roleManager.RoleExistsAsync("Both"))
            {
                var role = new IdentityRole("Both");
                await roleManager.CreateAsync(role);
            }
            if (!await roleManager.RoleExistsAsync("Unicorn"))
            {
                var role = new IdentityRole("Unicorn");
                await roleManager.CreateAsync(role);
            }
            if (await userManager.FindByNameAsync("SuperAdmin") == null)
            {
                var user = new ApplicationUser
                {
                    FirstName = "Gt",
                    LastName = "Admin",
                    UserName = "gtadmin@gmail.com",
                    Email = "gtadmin@gmail.com",
                    CreatedDate = DateTime.Now,
                    ApplicationUserTypeId = (int)Core.Common.Enums.ApplicationUserTypeEnum.SuperAdmin,
                    ApplicationUserStatusId = (int)Core.Common.Enums.ApplicationUserStatusEnum.Live,
                };

                var result = await userManager.CreateAsync(user, "GTAdmin@1234");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "SuperAdmin");
                }
            }
            if (await userManager.FindByNameAsync("Unicorn") == null)
            {
                var user = new ApplicationUser
                {
                    FirstName = "Chris",
                    LastName = "Y",
                    UserName = "chris@gmail.com",
                    Email = "chris@gmail.com",
                    CreatedDate = DateTime.Now,
                    ApplicationUserTypeId = (int)Core.Common.Enums.ApplicationUserTypeEnum.Unicorn,
                    ApplicationUserStatusId = (int)Core.Common.Enums.ApplicationUserStatusEnum.Live,
                };

                var result = await userManager.CreateAsync(user, "Chris@1234");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Unicorn");
                }
            }
        }
    }
}
