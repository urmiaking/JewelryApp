using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Data.Models;

public class ApplicationUserClaim : IdentityUserClaim<Guid>
{
    public virtual ApplicationUser User { get; set; } = default!;
}