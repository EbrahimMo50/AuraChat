namespace AuraChat.Entities;

public class GroupSettings
{
    public bool AdminOnlyMessages { get; set; } = false;
    public bool IsVisible { get; set; } = true;
    public bool AllowMembersToModifyInfo { get; set; } = false;
}
