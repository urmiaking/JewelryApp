namespace JewelryApp.Models.Dtos;

public record UserTokenDto(string Token, Guid RefreshToken)
{

}