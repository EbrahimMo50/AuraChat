using AuraChat.DTOs;

namespace AuraChat.Services.AuthServices;

public interface IAuthService
{
    public Task<LoginResultDto> LoginAsync(LoginDto loginDto);
    public LoginResultDto ConfirmLogin(int userId, string otp);
    public Task RegisterAsync(RegisterRequestDto registerDto);
    public Task ConfirmRegisterAsync(RegisterConfirmDto registerConfirmDto);
    public Task<ChangePassResultDto> ChangePasswordAsync(int userId, ChangePassDto changePassDto);
    public Task<ChangePassResultDto> ConfirmChangePasswordAsync(int userId,string otp);
    public Task ForgottenPasswordRequestAsync(string email);
    public Task ResetPasswordAsync(string token, string newPassword);
}
