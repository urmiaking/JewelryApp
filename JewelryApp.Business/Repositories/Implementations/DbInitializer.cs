using JewelryApp.Business.Repositories.Interfaces;
using JewelryApp.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Business.Repositories.Implementations;

public class DbInitializer : IDbInitializer
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DbInitializer(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public void Initialize()
    {
        var user = _userManager.FindByNameAsync("admin").GetAwaiter().GetResult();

        if (user is not null) 
            return;

        var result = _userManager.CreateAsync(new ApplicationUser
        {
            Name = "حسن فانی",
            UserName = "admin",
            Email = "admin@admin.com",
            EmailConfirmed = true
        }, "admin").GetAwaiter().GetResult();

        if (result.Succeeded)
            return;
        
        throw new Exception("Error on creating user");
    }
}
