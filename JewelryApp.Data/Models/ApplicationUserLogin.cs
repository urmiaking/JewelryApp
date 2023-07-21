using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Data.Models;

public class ApplicationUserLogin : IdentityUserLogin<Guid>
{
    public virtual ApplicationUser User { get; set; } = default!;
}