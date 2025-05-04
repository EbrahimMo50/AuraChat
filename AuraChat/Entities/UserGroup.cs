using Microsoft.EntityFrameworkCore;

namespace AuraChat.Entities;

/// <summary>
/// Represents a user's membership in a group and navigate the relationship between the two entities (M:N)
/// </summary>
public class UserGroup
{
    public int Id { get; private set; }
    public int UserId { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public User User { get; set; } = null!;
    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;
    public GroupRole Role { get; set; }
}

public enum GroupRole
{
    Admin,
    Member
}