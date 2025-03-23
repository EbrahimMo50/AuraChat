using System.Text.RegularExpressions;

namespace AuraChat.Entities;

public class User
{
    public int Id {  get; private set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public string HashedPassword { get; set; } = null!;
    public string? Bio {  get; set; }
    public string? ProfilePictureGuid { get; set; }
    public ICollection<Chat> Chats { get; set; } = [];
    public ICollection<UserGroup> UsersGroups { get; set; } = [];
    public ICollection<Group> Groups { get; set; } = [];
    public ICollection<User> Friends { get; set; } = [];
    public UserSettings UserSettings { get; set; } = new();
}
