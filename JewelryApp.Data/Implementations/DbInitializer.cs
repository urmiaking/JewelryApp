using JewelryApp.Common.Constants;
using JewelryApp.Data.Interfaces;
using JewelryApp.Data.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Business.Repositories.Implementations;

public class DbInitializer : IDbInitializer
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;

    public DbInitializer(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public void Initialize()
    {
        SeedIdentityUsers();
    }

    private void SeedIdentityUsers()
    {
        var adminUser = _userManager.FindByNameAsync(Identity.Admin).GetAwaiter().GetResult();
        var mainUser = _userManager.FindByNameAsync(Identity.MainUser).GetAwaiter().GetResult();
        var branchUser = _userManager.FindByNameAsync(Identity.SecondaryUser).GetAwaiter().GetResult();

        var adminRole = _roleManager.Roles.FirstOrDefault(x => x.Name == Identity.AdminRole);
        var mainUserRole = _roleManager.Roles.FirstOrDefault(x => x.Name == Identity.MainUserRole);
        var branchRole = _roleManager.Roles.FirstOrDefault(x => x.Name == Identity.SecondaryUserRole);

        if (adminUser is null)
        {
            adminUser = new AppUser
            {
                UserName = Identity.Admin,
                Email = Identity.AdminEmail,
                EmailConfirmed = true,
                Name = Identity.AdminDisplayName
            };

            _userManager.CreateAsync(adminUser, Identity.AdminPassword).Wait();
        }

        if (mainUser is null)
        {
            mainUser = new AppUser
            {
                UserName = Identity.MainUser,
                Email = Identity.MainUserEmail,
                EmailConfirmed = true,
                Name = Identity.MainUserDisplayName
            };

            _userManager.CreateAsync(mainUser, Identity.MainUserPassword).Wait();
        }

        if (branchUser is null)
        {
            branchUser = new AppUser
            {
                UserName = Identity.SecondaryUser,
                Email = Identity.SecondaryUserEmail,
                EmailConfirmed = true,
                Name = Identity.SecondaryUserDisplayName
            };

            _userManager.CreateAsync(branchUser, Identity.SecondaryUserPassword).Wait();
        }

        if (adminRole is null)
        {
            adminRole = new AppRole
            {
                Name = Identity.AdminRole
            };

            _roleManager.CreateAsync(adminRole).Wait();
        }

        if (mainUserRole is null)
        {
            mainUserRole = new AppRole
            {
                Name = Identity.MainUserRole
            };

            _roleManager.CreateAsync(mainUserRole).Wait();
        }

        if (branchRole is null)
        {
            branchRole = new AppRole
            {
                Name = Identity.SecondaryUserRole
            };

            _roleManager.CreateAsync(branchRole).Wait();
        }

        var isAdminRoleAssigned = _userManager.IsInRoleAsync(adminUser, Identity.AdminRole).GetAwaiter().GetResult();
        var isMainUserRoleAssigned = _userManager.IsInRoleAsync(mainUser, Identity.MainUserRole).GetAwaiter().GetResult();
        var isBranchRoleAssigned = _userManager.IsInRoleAsync(branchUser, Identity.SecondaryUserRole).GetAwaiter().GetResult();

        if (!isAdminRoleAssigned)
        {
            _userManager.AddToRoleAsync(adminUser, Identity.AdminRole).Wait();
        }

        if (!isMainUserRoleAssigned)
        {
            _userManager.AddToRoleAsync(mainUser, Identity.MainUserRole).Wait();
        }

        if (!isBranchRoleAssigned)
        {
            _userManager.AddToRoleAsync(branchUser, Identity.SecondaryUserRole).Wait();
        }
    }
}
