using JewelryApp.Data;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JewelryApp.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SettingsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public SettingsController(AppDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpPut]
    [Route("/changepassword")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto passwordDto)
    {
        var userName = User.Identity!.Name;

        var user = await _context.Users.FirstOrDefaultAsync(a => a.UserName == userName);

        if (user is null)
            return BadRequest();

        var result = await _userManager.ChangePasswordAsync(user, passwordDto.OldPassword, passwordDto.NewPassword);

        if (result.Succeeded)
            return Ok();
        
        return BadRequest();
    }
}
