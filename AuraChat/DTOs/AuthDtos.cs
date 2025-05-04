using System.ComponentModel.DataAnnotations;

namespace AuraChat.DTOs;

public class LoginDto
{
    [EmailAddress]
    public string Email { get; set; } = null!;
    [MinLength(4)]
    public string Password { get; set; } = null!;
}

public class LoginResultDto
{
    public string? AccessToken { get; set; } = null;
    //public string? RefreshToken { get; set; } = null;
    public bool TwoFactorRequired { get; set; } = true;
    public int UserId { get; set; }
}

public class ChangePassDto
{
    [MinLength(4)]
    public string OldPassword { get; set; } = null!;
    [MinLength(4)]
    public string NewPassword { get; set; } = null!;
}

public class ChangePassResultDto
{
    public bool TwoFactorRequired { get; set; } = true;
    public bool Confirmed { get; set; } = false;
}

public class RegisterRequestDto
{
    [EmailAddress]
    public string Email { get; set; } = null!;
    [MinLength(4)]
    public string Password { get; set; } = null!;
}

public class RegisterConfirmDto
{
    [EmailAddress]
    public string Email { get; set; } = null!;
    [RegularExpression(@"^[\p{L}\s]{2,20}$", ErrorMessage = "only letters and spaces allowed, max length 20 and min 2.")]
    public string UserName { get; set; } = null!;
    public string Otp { get; set; } = null!;
}
