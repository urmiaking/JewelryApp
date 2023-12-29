using JewelryApp.Core.Constants;
using JewelryApp.Core.DomainModels;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Interfaces;
using JewelryApp.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Identity;
using JewelryApp.Shared.Attributes;

namespace JewelryApp.Infrastructure.Implementations;

[ScopedService<IDbInitializer>]
public class DbInitializer : IDbInitializer
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IProductCategoryRepository _productCategoryRepository;

    public DbInitializer(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IProductCategoryRepository productCategoryRepository)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _productCategoryRepository = productCategoryRepository;
    }

    public void Initialize()
    {
        SeedIdentityUsers();
        SeedProductCategories();
    }

    private void SeedProductCategories()
    {
        if (!_productCategoryRepository.Get(useAuthentication: false).Any())
        {
            var initialCategories = new List<ProductCategory>
            {
                new() { Name = Data.ProductCategories.HalfSet },
                new() { Name = Data.ProductCategories.Medal },
                new() { Name = Data.ProductCategories.Chain },
                new() { Name = Data.ProductCategories.Service },
                new() { Name = Data.ProductCategories.Ring },
                new() { Name = Data.ProductCategories.Bracelet }
            };
            _productCategoryRepository.AddRangeAsync(initialCategories, useAuthentication: false).Wait();
        }
    }

    private void SeedIdentityUsers()
    {
        var adminUser = _userManager.FindByNameAsync(Data.Identity.Admin).GetAwaiter().GetResult();
        var mainUser = _userManager.FindByNameAsync(Data.Identity.MainUser).GetAwaiter().GetResult();
        var branchUser = _userManager.FindByNameAsync(Data.Identity.SecondaryUser).GetAwaiter().GetResult();

        var adminRole = _roleManager.Roles.FirstOrDefault(x => x.Name == Data.Identity.AdminRole);
        var mainUserRole = _roleManager.Roles.FirstOrDefault(x => x.Name == Data.Identity.MainUserRole);
        var branchRole = _roleManager.Roles.FirstOrDefault(x => x.Name == Data.Identity.SecondaryUserRole);

        if (adminUser is null)
        {
            adminUser = new AppUser
            {
                UserName = Data.Identity.Admin,
                Email = Data.Identity.AdminEmail,
                EmailConfirmed = true,
                Name = Data.Identity.AdminDisplayName
            };

            _userManager.CreateAsync(adminUser, Data.Identity.AdminPassword).Wait();
        }

        if (mainUser is null)
        {
            mainUser = new AppUser
            {
                UserName = Data.Identity.MainUser,
                Email = Data.Identity.MainUserEmail,
                EmailConfirmed = true,
                Name = Data.Identity.MainUserDisplayName
            };

            _userManager.CreateAsync(mainUser, Data.Identity.MainUserPassword).Wait();
        }

        if (branchUser is null)
        {
            branchUser = new AppUser
            {
                UserName = Data.Identity.SecondaryUser,
                Email = Data.Identity.SecondaryUserEmail,
                EmailConfirmed = true,
                Name = Data.Identity.SecondaryUserDisplayName
            };

            _userManager.CreateAsync(branchUser, Data.Identity.SecondaryUserPassword).Wait();
        }

        if (adminRole is null)
        {
            adminRole = new AppRole
            {
                Name = Data.Identity.AdminRole
            };

            _roleManager.CreateAsync(adminRole).Wait();
        }

        if (mainUserRole is null)
        {
            mainUserRole = new AppRole
            {
                Name = Data.Identity.MainUserRole
            };

            _roleManager.CreateAsync(mainUserRole).Wait();
        }

        if (branchRole is null)
        {
            branchRole = new AppRole
            {
                Name = Data.Identity.SecondaryUserRole
            };

            _roleManager.CreateAsync(branchRole).Wait();
        }

        var isAdminRoleAssigned = _userManager.IsInRoleAsync(adminUser, Data.Identity.AdminRole).GetAwaiter().GetResult();
        var isMainUserRoleAssigned = _userManager.IsInRoleAsync(mainUser, Data.Identity.MainUserRole).GetAwaiter().GetResult();
        var isBranchRoleAssigned = _userManager.IsInRoleAsync(branchUser, Data.Identity.SecondaryUserRole).GetAwaiter().GetResult();

        if (!isAdminRoleAssigned)
        {
            _userManager.AddToRoleAsync(adminUser, Data.Identity.AdminRole).Wait();
        }

        if (!isMainUserRoleAssigned)
        {
            _userManager.AddToRoleAsync(mainUser, Data.Identity.MainUserRole).Wait();
        }

        if (!isBranchRoleAssigned)
        {
            _userManager.AddToRoleAsync(branchUser, Data.Identity.SecondaryUserRole).Wait();
        }
    }
}
