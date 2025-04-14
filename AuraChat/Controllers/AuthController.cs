using AuraChat.DTOs;
using AuraChat.Services.AuthServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuraChat.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var result = await authService.LoginAsync(loginDto);
        return Ok(result);
    }

    [HttpPost("confirm-login")]
    public IActionResult ConfirmLogin(int userId, string otp)
    {
        var result = authService.ConfirmLogin(userId, otp);
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<IActionResult> SignUp(RegisterRequestDto registerRequestDto)
    {
        await authService.RegisterAsync(registerRequestDto); 
        return Ok();
    }

    [HttpPost("confirm-register")]
    public async Task<IActionResult> ConfirmSignUp(RegisterConfirmDto registerConfirmDto)
    {
        await authService.ConfirmRegisterAsync(registerConfirmDto);
        return Ok();
    }

    [Authorize]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePassDto changePassDto)
    {
        var result = await authService.ChangePasswordAsync(changePassDto);
        return Ok(result);
    }
    [Authorize]
    [HttpPut("confirm-change-password")]
    public async Task<IActionResult> ConfirmChangePassword(string otp)
    {
        var userId = int.Parse(HttpContext.User.FindFirst("Id")!.Value);
        var result = await authService.ConfirmChangePasswordAsync(userId, otp);
        return Ok(result);
    }
}
