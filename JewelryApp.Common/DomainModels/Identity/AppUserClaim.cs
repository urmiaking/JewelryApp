using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Core.DomainModels.Identity;

public class AppUserClaim : IdentityUserClaim<Guid>
{
    public virtual AppUser User { get; set; } = default!;
}