namespace JewelryApp.Models.Dtos.Authentication;

public record UserTokenDto(string Token, Guid RefreshToken)
{

}