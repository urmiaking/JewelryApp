using JewelryApp.Core.Constants;
using JewelryApp.Core.DomainModels.Identity;
using JewelryApp.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using JewelryApp.Shared.Attributes;

namespace JewelryApp.Infrastructure.Implementations;

[ScopedService<IElevatedAccessService>]
public class ElevatedAccessService : IElevatedAccessService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<AppUser> _userManager;
    public ElevatedAccessService(IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
    {
        _httpContextAccessor = httpContextAccessor;
        _userManager = userManager;
    }

    public bool IsAdminUser()
    {
        return _httpContextAccessor.HttpContext?.User.IsInRole(Data.Identity.AdminRole) ?? false;
    }

    public bool IsMainUser()
    {
        return _httpContextAccessor.HttpContext?.User.IsInRole(Data.Identity.MainUserRole) ?? false;
    }

    public Guid? GetUserId()
    {
        var userClaim = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x=> x.Type.Equals(ClaimTypes.NameIdentifier));

        if (userClaim is null)
            return null;

        return new Guid(userClaim.Value);
    }
}