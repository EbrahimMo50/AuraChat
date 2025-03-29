using AuraChat.DTOs;

namespace AuraChat.Services.AuthServices;

public interface IAuthService
{
    /// <summary>
    /// return type is void thus it is required to login before returning the token
    /// </summary>
    /// <param name="loginDto"></param>
    public Task<LoginResultDto> LoginAsync(LoginDto loginDto);
    public Task RegisterAsync(RegisterRequestDto registerDto);
    public void ChangePassword(ChangePassDto changePassDto);
    public LoginResultDto ConfirmLogin(int userId, string otp);
    public Task ConfirmRegisterAsync(RegisterConfirmDto registerConfirmDto);
    public void ConfirmChangePassword(int userId,string otp);
}
