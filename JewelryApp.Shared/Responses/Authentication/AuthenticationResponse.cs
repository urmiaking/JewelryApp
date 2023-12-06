namespace JewelryApp.Shared.Responses.Authentication;

public record AuthenticationResponse (string Token, Guid RefreshToken);