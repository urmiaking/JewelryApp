using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Data.Models;

public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
{
    public virtual ApplicationRole Role { get; set; } = default!;
}