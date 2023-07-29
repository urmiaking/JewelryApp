using JewelryApp.Data;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos;
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

    [HttpGet]
    [Route("/apikeys")]
    public async Task<IActionResult> GetApiKeys()
    {
        var apiKeys = (await _context.ApiKeys.OrderByDescending(a => a.AddDateTime).ToListAsync()).Select(a => new ApiKeyDto() { Key = a.Key, IsActive = a.IsActive});

        return Ok(apiKeys);
    }

    [HttpPost]
    [Route("/updateapikey")]
    public async Task<IActionResult> UpdateApiKey(ApiKeyDto apiKeyDto)
    {
        if (string.IsNullOrEmpty(apiKeyDto.Key))
        {
            return BadRequest();
        }

        var lastApiKey = await _context.ApiKeys.OrderByDescending(a => a.AddDateTime).FirstOrDefaultAsync(a => a.IsActive);

        if (lastApiKey != null)
        {
            lastApiKey.IsActive = !apiKeyDto.IsActive;
            _context.ApiKeys.Update(lastApiKey);
        }

        await _context.ApiKeys.AddAsync(new ApiKey() { AddDateTime = DateTime.Now, Key = apiKeyDto.Key, IsActive = apiKeyDto.IsActive });
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
    [Route("/setactiveapikey")]
    public async Task<IActionResult> SetActiveApiKey(ApiKeyDto apiKeyDto)
    {
        if (apiKeyDto is null)
        {
            return BadRequest();
        }

        var apiKey = await _context.ApiKeys.FirstOrDefaultAsync(a => a.Key == apiKeyDto.Key);
        var currentActiveApiKey = await _context.ApiKeys.FirstOrDefaultAsync(a => a.IsActive);

        if (apiKey == null || currentActiveApiKey == null)
        {
            return BadRequest();
        }

        if (apiKey.Id == currentActiveApiKey.Id)
        {
            return Ok();
        }


        apiKey.IsActive = true;
        currentActiveApiKey.IsActive = false;

        _context.ApiKeys.Update(currentActiveApiKey);
        _context.ApiKeys.Update(apiKey);

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost]
    [Route("/changepassword")]
    public async Task<IActionResult> ChangePassowrd(ChangePasswordDto passwordDto)
    {
        if (passwordDto is null)
        {
            return BadRequest();
        }

        var userName = User.Identity!.Name;

        var user = await _context.Users.FirstOrDefaultAsync(a => a.UserName == userName);

        if (user is null)
        {
            return BadRequest();
        }

        var result = await _userManager.ChangePasswordAsync(user, passwordDto.OldPassword, passwordDto.NewPassword);

        if (result.Succeeded)
        {
            return Ok();
        }

        return BadRequest();
    }
}
