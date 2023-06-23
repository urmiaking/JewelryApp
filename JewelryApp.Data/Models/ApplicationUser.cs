using Microsoft.AspNetCore.Identity;

namespace JewelryApp.Data.Models;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
}