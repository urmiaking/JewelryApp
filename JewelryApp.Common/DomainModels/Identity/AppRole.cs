using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Core.DomainModels.Identity;

public class AppRole : IdentityRole<Guid>
{
    public virtual ICollection<AppUserRole>? UserRoles { get; set; }
    public virtual ICollection<AppRoleClaim>? Claims { get; set; }
}
