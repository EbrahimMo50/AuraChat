namespace AuraChat.Policies;

/// <summary>
/// a class that holds all the names of the policies registered in the application DI to be used as strognly typed on controllers
/// </summary>
public static class RegisteredPolicies
{
    public const string GroupMemberPolicy = "GroupMemberPolicy";
    public const string FriendsPolicy = "FriendsPolicy";
    public const string GroupAdminPolicy = "GroupAdminPolicy";
}
