using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Data.Models.Identity;

public class AppUserClaim : IdentityUserClaim<Guid>
{
    public virtual AppUser User { get; set; } = default!;
}