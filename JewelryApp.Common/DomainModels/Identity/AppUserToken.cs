using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Core.DomainModels.Identity;

public class AppUserToken : IdentityUserToken<Guid>
{
    public virtual AppUser User { get; set; } = default!;
}