using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Core.DomainModels.Identity;

public class AppRoleClaim : IdentityRoleClaim<Guid>
{
    public virtual AppRole Role { get; set; } = default!;
}