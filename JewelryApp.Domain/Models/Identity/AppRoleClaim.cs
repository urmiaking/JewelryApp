using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Data.Models.Identity;

public class AppRoleClaim : IdentityRoleClaim<Guid>
{
    public virtual AppRole Role { get; set; } = default!;
}