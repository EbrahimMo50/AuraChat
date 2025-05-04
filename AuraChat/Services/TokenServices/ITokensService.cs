using AuraChat.Entities;

namespace AuraChat.Services.TokenServices;

public interface ITokensService
{
    public string GenerateAccessToken(User user);
    // TODO public string GenerateRefreshToken();
}
