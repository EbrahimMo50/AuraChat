namespace AuraChat.Entities;

public class Group
{
    public int Id { get; private set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? GroupPictureGuid { get; set; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public ICollection<UserGroup> Members { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
    public GroupSettings Settings { get; set; } = new();
    public int CreatorId { get; set; }
    public User Creator { get; set; } = null!;
}