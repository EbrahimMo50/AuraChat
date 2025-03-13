namespace AuraChat.Entities;

public class UserSettings
{
    public bool IsVisible { get; set; } = true;
    public bool FriendsOnlyMessages { get; set; } = false;
    public bool FriendsOnlyGroupInvite { get; set; } = false;
    public bool TwoFactorAuthEnabled { get; set; } = true;
}
