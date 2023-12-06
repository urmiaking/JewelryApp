namespace JewelryApp.Models.Dtos.AuthenticationDtos;

public record UserTokenDto(string Token, Guid RefreshToken)
{

}