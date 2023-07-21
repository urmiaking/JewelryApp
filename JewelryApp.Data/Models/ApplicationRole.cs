using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Data.Models;

public class ApplicationRole : IdentityRole<Guid>
{
    public virtual ICollection<ApplicationUserRole>? UserRoles { get; set; }
    public virtual ICollection<ApplicationRoleClaim>? Claims { get; set; }
}
