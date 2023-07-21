using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Data.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public string Name { get; set; }

    public virtual ICollection<ApplicationUserClaim> Claims { get; set; } = default!;
    public virtual ICollection<ApplicationUserLogin> Logins { get; set; } = default!;
    public virtual ICollection<ApplicationUserToken> Tokens { get; set; } = default!;
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = default!;
}