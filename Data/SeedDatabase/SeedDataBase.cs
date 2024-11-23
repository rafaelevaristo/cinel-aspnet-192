using Microsoft.AspNetCore.Identity;

namespace mvc.Data.SeedDataBase;
 public class SeedDatabase
    {
        public static void Seed(ApplicationDbContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager).Wait();
            SeedUsers(userManager).Wait();
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
    // CREATE ADMIN
    var roleCheck = await roleManager.RoleExistsAsync(MVCConstants.ROLES.ADMIN);
    if (!roleCheck)
    {
        var adminRole = new IdentityRole
        {
            Name = MVCConstants.ROLES.ADMIN
        };
        await roleManager.CreateAsync(adminRole);
    }

    // CREATE DRIVER
    roleCheck = await roleManager.RoleExistsAsync(MVCConstants.ROLES.DRIVER);
    if (!roleCheck)
    {
        var driverRole = new IdentityRole
        {
            Name = MVCConstants.ROLES.DRIVER
        };
        await roleManager.CreateAsync(driverRole);
    }

    // CREATE ADMINISTRATIVE
    roleCheck = await roleManager.RoleExistsAsync(MVCConstants.ROLES.ADMINISTRATIVE);
    if (!roleCheck)
    {
        var administrativeRole = new IdentityRole
        {
            Name = MVCConstants.ROLES.ADMINISTRATIVE
        };
        await roleManager.CreateAsync(administrativeRole);
    }
}

private static async Task SeedUsers(UserManager<IdentityUser> userManager)
{
    // Seed Admin User
    var dbAdmin = await userManager.FindByNameAsync(MVCConstants.USERS.ADMIN.USERNAME);

    if (dbAdmin == null)
    {
        IdentityUser userAdmin = new IdentityUser
        {
            UserName = MVCConstants.USERS.ADMIN.USERNAME,
            Email = MVCConstants.USERS.ADMIN.EMAIL
        };

        var result = await userManager.CreateAsync(userAdmin, MVCConstants.USERS.ADMIN.PASSWORD);

        if (result.Succeeded)
        {
            dbAdmin = await userManager.FindByNameAsync(MVCConstants.USERS.ADMIN.USERNAME);
            await userManager.AddToRoleAsync(dbAdmin, MVCConstants.ROLES.ADMIN);
        }
    }

    // Seed Driver User
    var dbDriver = await userManager.FindByNameAsync(MVCConstants.USERS.DRIVER.USERNAME);

    if (dbDriver == null)
    {
        IdentityUser userDriver = new IdentityUser
        {
            UserName = MVCConstants.USERS.DRIVER.USERNAME,
            Email = MVCConstants.USERS.DRIVER.EMAIL
        };

        var result = await userManager.CreateAsync(userDriver, MVCConstants.USERS.DRIVER.PASSWORD);

        if (result.Succeeded)
        {
            dbDriver = await userManager.FindByNameAsync(MVCConstants.USERS.DRIVER.USERNAME);
            await userManager.AddToRoleAsync(dbDriver, MVCConstants.ROLES.DRIVER);
        }
    }

    // Seed Administrative User
    var dbAdministrative = await userManager.FindByNameAsync(MVCConstants.USERS.ADMINISTRATIVE.USERNAME);

    if (dbAdministrative == null)
    {
        IdentityUser userAdministrative = new IdentityUser
        {
            UserName = MVCConstants.USERS.ADMINISTRATIVE.USERNAME,
            Email = MVCConstants.USERS.ADMINISTRATIVE.EMAIL
        };

        var result = await userManager.CreateAsync(userAdministrative, MVCConstants.USERS.ADMINISTRATIVE.PASSWORD);

        if (result.Succeeded)
        {
            dbAdministrative = await userManager.FindByNameAsync(MVCConstants.USERS.ADMINISTRATIVE.USERNAME);
            await userManager.AddToRoleAsync(dbAdministrative, MVCConstants.ROLES.ADMINISTRATIVE);
        }
    }
}

    }