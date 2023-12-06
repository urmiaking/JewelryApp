using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Data.Models.Identity;

public class AppUserLogin : IdentityUserLogin<Guid>
{
    public virtual AppUser User { get; set; } = default!;
}