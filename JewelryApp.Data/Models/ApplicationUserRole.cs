using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Data.Models;

public class ApplicationUserRole : IdentityUserRole<Guid>
{
    public virtual ApplicationUser User { get; set; } = default!;
    public virtual ApplicationRole Role { get; set; } = default!;
}
