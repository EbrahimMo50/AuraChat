using AuraChat.DTOs;
using AuraChat.Entities;
using AuraChat.Exceptions;
using AuraChat.Models;
using AuraChat.Repositries.UserRepo;
using AuraChat.Services.EmailServices;
using AuraChat.Services.TokenServices;
using AuraChat.Templates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using System.Security.Cryptography;
using System.Text;

namespace AuraChat.Services.AuthServices;

public class AuthService(IMemoryCache cache, IStringLocalizer<AuthService> stringLocalizer, IUserRepo userRepo, 
    ITokensService tokenService, IEmailService emailService, IConfiguration configuration) : IAuthService
{
    #region Login
    public async Task<LoginResultDto> LoginAsync(LoginDto loginDto)
    {
        var user = await userRepo.GetByEmailAsync(loginDto.Email) ?? throw new NotFoundException(stringLocalizer["Email not found"]);

        if (Hash(user.Salt, loginDto.Password) == user.HashedPassword)
        {
            if (!user.UserSettings.TwoFactorAuthEnabled)
                return new LoginResultDto() { AccessToken = tokenService.GenerateAccessToken(user), UserId = user.Id, TwoFactorRequired = false };

            var verificationCode = new Random().Next(100000, 999999).ToString();

            // check if the user has sent before to prevent memory over loading
            var oldCache = cache.Get<dynamic>($"login:{user.Id}");

            if (oldCache != null)
            {
                verificationCode = oldCache.VerificationCode;
                cache.Remove($"login:{user.Id}");
            }

            cache.Set(
                $"login:{user.Id}",
                new { User = user, VerificationCode = verificationCode },
                DateTime.Now.AddMinutes(15));

            // sending the otp to the user
            emailService.SendEmail(new EmailModel()
            {
                Header = "OTP for Login",
                Body = EmailTemplates.GetOtpEmailBody(verificationCode),
                ReicieverEmail = user.Email
            });

            return new LoginResultDto() { UserId = user.Id };
        }

        throw new BadRequestException(stringLocalizer["Password is incorrect"]);
    }
    public LoginResultDto ConfirmLogin(int userId, string otp)
    {
        var cacheKey = $"login:{userId}";
        var cachedData = cache.Get<dynamic>(cacheKey) ?? throw new NotFoundException(stringLocalizer["could not find given entry"]);
        if (cachedData.VerificationCode == otp)
        {
            cache.Remove(cacheKey);
            return new LoginResultDto() { AccessToken = tokenService.GenerateAccessToken(cachedData.User), TwoFactorRequired = true, UserId = userId };
        }
        throw new BadRequestException(stringLocalizer["could not find proposed request or expired"]);
    }
    #endregion

    #region Registeration
    public async Task RegisterAsync(RegisterRequestDto registerDto)
    {
        if (cache.Get<dynamic>($"registerEmail:{registerDto.Email}") != null)
            throw new ConflictException(stringLocalizer["a request with this email already exists please confirm using otp in ur mail"]);

        if(await userRepo.GetByEmailAsync(registerDto.Email) != null)
            throw new ConflictException(stringLocalizer["email is already used by another user"]);

        var verificationCode = new Random().Next(100000, 999999).ToString();

        cache.Set(
            $"registerEmail:{registerDto.Email}",
            new { RegisterDto = registerDto, VerificationCode = verificationCode },
            DateTime.Now.AddMinutes(15));

        emailService.SendEmail(new EmailModel()
        {
            Header = "Email confirmation",
            Body = EmailTemplates.GetOtpEmailBody(verificationCode),
            ReicieverEmail = registerDto.Email
        });
    }
    public async Task ConfirmRegisterAsync(RegisterConfirmDto registerConfirmDto)
    {
        var cacheKey = $"registerEmail:{registerConfirmDto.Email}";
        var cachedData = cache.Get<dynamic>(cacheKey) ?? throw new NotFoundException(stringLocalizer["could not find given entry"]);

        if (cachedData.VerificationCode == registerConfirmDto.Otp)
        {
            cache.Remove(cacheKey);

            if (await userRepo.GetByEmailAsync(registerConfirmDto.Email) != null)
                throw new ConflictException(stringLocalizer["email is already used by another user"]);

            var salt = GenerateSalt();
            var hashedPassword = Hash(salt, cachedData.RegisterDto.Password);

            await userRepo.AddAsync(new User() { Email = registerConfirmDto.Email, Name = registerConfirmDto.UserName, Salt = salt, HashedPassword = hashedPassword });
            return;
        }
        throw new BadRequestException(stringLocalizer["could not find proposed request or expired"]);
    }
    #endregion

    #region ChangePassword
    public async Task<ChangePassResultDto> ChangePasswordAsync(int userId, ChangePassDto changePassDto)
    {
        var user = userRepo.GetByIdAsync(userId).Result ?? throw new NotFoundException(stringLocalizer["User Not Found"]);
        if (user.HashedPassword == Hash(user.Salt, changePassDto.OldPassword))
        {
            if (!user.UserSettings.TwoFactorAuthEnabled)
            {
                var salt = GenerateSalt();
                var hashedPassword = Hash(salt, changePassDto.NewPassword);
                user.HashedPassword = hashedPassword;
                user.Salt = salt;
                user.UserSettings.PasswordChangeCounter+=1;
                await userRepo.UpdateAsync(user);
                return new ChangePassResultDto() { Confirmed = true, TwoFactorRequired = false };
            }

            // if the user has two factor auth enabled

            var verificationCode = new Random().Next(100000, 999999).ToString();

            var oldCache = cache.Get<dynamic>($"change_password:{user.Id}");
            if (oldCache != null)
            {
                verificationCode = oldCache.VerificationCode;
                cache.Remove($"change_password:{user.Id}");
            }

            cache.Set(
                $"change_password:{user.Id}",
                new { VerificationCode = verificationCode, changePassDto.NewPassword },
                DateTime.Now.AddMinutes(15));
            // sending the otp to the user
            emailService.SendEmail(new EmailModel()
            {
                Header = "OTP for Changing Password",
                Body = EmailTemplates.GetOtpEmailBody(verificationCode),
                ReicieverEmail = user.Email
            });

            return new ChangePassResultDto() { Confirmed = false, TwoFactorRequired = true };
        }
        else
        {
            throw new BadRequestException(stringLocalizer["Password is incorrect"]);
        }
        throw new NotFoundException(stringLocalizer["could find propsed user"]);    
    }

    // for security integration tokens shall be expired from all instances when a user changes his password using a stamp of password version
    public async Task<ChangePassResultDto> ConfirmChangePasswordAsync(int userId, string otp)
    {
        var cacheKey = $"change_password:{userId}";
        var cachedData = cache.Get<dynamic>(cacheKey) ?? throw new NotFoundException(stringLocalizer["could not find given entry"]);

        if (cachedData.VerificationCode == otp)
        {
            cache.Remove(cacheKey);

            var user = await userRepo.GetByIdAsync(userId) ?? throw new NotFoundException(stringLocalizer["user not found"]);
            var salt = GenerateSalt();
            var hashedPassword = Hash(salt, cachedData.NewPassword);
            user.HashedPassword = hashedPassword;
            user.Salt = salt;
            user.UserSettings.PasswordChangeCounter += 1;
            await userRepo.UpdateAsync(user);
            return new ChangePassResultDto() { Confirmed = true, TwoFactorRequired = true };
        }

        throw new BadRequestException(stringLocalizer["could not find proposed request or expired"]);
    }
    #endregion

    #region ForgottenPassword
    public async Task ForgottenPasswordRequestAsync(string email)
    {
        if (cache.Get<dynamic>($"requested_reset:{email}") != null)
            return;

        var user = await userRepo.GetByEmailAsync(email) ?? throw new NotFoundException(stringLocalizer["Email not found"]);
        var validationToken = GenerateSecureBase64Token(64);

        cache.Set(
            $"forgot_password:{validationToken}",
            new { Email = email }, 
            DateTime.Now.AddMinutes(60));

        // an entity to register that users requested or not to prevent flooding requests and spams
        cache.Set($"requested_reset:{email}", new {}, DateTime.Now.AddMinutes(60));

        // will change to the front server once deployed as redirection
        var resetPasswordLink = "suposdly-front-web-server-address" + "/auth/verify-password-reset?token=" + validationToken;

        emailService.SendEmail(new EmailModel() 
        { ReicieverEmail = email, Body = EmailTemplates.GetForgottenPasswordEmailBody(resetPasswordLink), Header = "Password Reset Request"});
    }

    public async Task ResetPasswordAsync(string token, string newPassword)
    {
        var cachedData = cache.Get<dynamic>("forgot_password:" + token) ?? throw new NotFoundException(stringLocalizer["could not find proposed request or expired"]);

        cache.Remove("forgot_password:" + token);
        cache.Remove($"requested_reset:{cachedData.Email}");

        var user = await userRepo.GetByEmailAsync(cachedData.Email) ?? throw new NotFoundException(stringLocalizer["user not found"]);   // to prevent tampering just in case
        var salt = GenerateSalt();
        var hashedPassword = Hash(salt, newPassword);
        user.HashedPassword = hashedPassword;
        user.Salt = salt;
        user.UserSettings.PasswordChangeCounter += 1;
        await userRepo.UpdateAsync(user);
    }
    #endregion

    private static string GenerateSecureBase64Token(int byteLength)
    {
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] randomBytes = new byte[byteLength];
            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes)
                         .Replace("+", "-")  // URL-safe
                         .Replace("/", "_")
                         .Replace("=", "");  // Remove padding
        }
    }

    private static string Hash(string salt, string pass)
    {
        var InputBytes = Encoding.UTF8.GetBytes(salt + pass);
        return Convert.ToHexString(SHA256.HashData(InputBytes));
    }

    private static string GenerateSalt()
    {
        var random = new Random();
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var Salt = new string(Enumerable.Repeat(chars, 32)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        return Salt;
    }
}
