using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Core.DomainModels.Identity;

public class AppUserLogin : IdentityUserLogin<Guid>
{
    public virtual AppUser User { get; set; } = default!;
}