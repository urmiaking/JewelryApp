using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Core.DomainModels.Identity;

public class AppUser : IdentityUser<Guid>
{
    public string Name { get; set; } = null!;

    public virtual ICollection<AppUserClaim> Claims { get; set; } = default!;
    public virtual ICollection<AppUserLogin> Logins { get; set; } = default!;
    public virtual ICollection<AppUserToken> Tokens { get; set; } = default!;
    public virtual ICollection<AppUserRole> UserRoles { get; set; } = default!;
}