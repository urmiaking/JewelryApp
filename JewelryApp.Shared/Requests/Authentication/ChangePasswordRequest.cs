namespace JewelryApp.Shared.Requests.Authentication;

public record ChangePasswordRequest (string UserName, string OldPassword, string NewPassword, string ConfirmNewPassword);