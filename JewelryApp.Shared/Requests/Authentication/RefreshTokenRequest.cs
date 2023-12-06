namespace JewelryApp.Shared.Requests.Authentication;

public record RefreshTokenRequest(string Token, Guid RefreshToken);
