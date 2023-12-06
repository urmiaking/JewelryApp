namespace JewelryApp.Common.Settings;

public sealed class JwtSettings
{
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public string Key { get; set; } = default!;
    public TimeSpan TokenLifeTime { get; set; }
    public TimeSpan RefreshTokenLifeTime { get; set; }
}