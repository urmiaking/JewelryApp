using JewelryApp.Business.Repositories.Interfaces;
using JewelryApp.Data;
using JewelryApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Business.Repositories.Implementations;

public class DbInitializer : IDbInitializer
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly AppDbContext _db;

    public DbInitializer(UserManager<ApplicationUser> userManager, AppDbContext db, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _db = db;
        _roleManager = roleManager;
    }

    public void Initialize()
    {
        SeedApiKeys(_db).GetAwaiter().GetResult();
        SeedIdentityAsync(_userManager, _roleManager).GetAwaiter().GetResult();
    }

    private async Task SeedIdentityAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync("Administrators"))
            await roleManager.CreateAsync(new ApplicationRole { Name = "Administrators" });

        var adminUser = await userManager.FindByNameAsync("admin");
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = "admin",
            };
            await userManager.CreateAsync(adminUser, "admin");
        }

        if (!await userManager.IsInRoleAsync(adminUser, "Administrators"))
            await userManager.AddToRoleAsync(adminUser, "Administrators");
    }

    private async Task SeedApiKeys(AppDbContext context)
    {
        var apiKeyExist = await _db.ApiKeys.AnyAsync();
        if (!apiKeyExist)
        {
            var apiKey = new ApiKey
            {
                AddDateTime = DateTime.Now,
                Key = "freeM8O5H9tyfnlOZdTjAl49Aiv86j90"
            };

            await context.ApiKeys.AddAsync(apiKey);
            await context.SaveChangesAsync();
        }
    }
}
