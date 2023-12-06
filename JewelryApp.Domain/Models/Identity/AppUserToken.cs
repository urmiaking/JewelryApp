using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Data.Models.Identity;

public class AppUserToken : IdentityUserToken<Guid>
{
    public virtual AppUser User { get; set; } = default!;
}