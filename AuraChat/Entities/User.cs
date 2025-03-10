namespace AuraChat.Entities;

public class User
{
    public int Id {  get; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Salt { get; set; } = null!;
    public string HashedPassword { get; set; } = null!;
    public string? Bio {  get; set; }
    public string? ProfilePictureGUID { get; set; }
    public List<Chat> Chats { get; set; } = [];
    public List<Group> Groups { get; set; } = [];
    public List<User> Friends { get; set; } = [];
    public UserSettings UserSettings { get; set; } = new();
}
