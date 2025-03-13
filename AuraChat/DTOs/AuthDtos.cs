using System.ComponentModel.DataAnnotations;

namespace AuraChat.DTOs;

public class LoginDto
{
    [EmailAddress]
    public string Email { get; set; } = null!;
    [MinLength(4)]
    public string Password { get; set; } = null!;
}

public class ChangePassDto
{
    [EmailAddress]
    public string Email { get; set; } = null!;
    [MinLength(4)]
    public string OldPassword { get; set; } = null!;
    [MinLength(4)]
    public string NewPassword { get; set; } = null!;
}

public class RegisterDto
{
    [RegularExpression(@"^[\p{L}\s]{2,20}$", ErrorMessage = "only letters and spaces allowed, max length 20 and min 2.")]
    public string UserName { get; set; } = null!;
    [EmailAddress]
    public string Email { get; set; } = null!;
    [MinLength(4)]
    public string Password { get; set; } = null!;
    [MinLength(4)]
    [Compare(nameof(Password), ErrorMessage = "passwords do not match")]
    public string ConfirmPassword { get; set; } = null!;
}