﻿namespace AuraChat.Entities;

public class Group
{
    public int Id { get; private set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string? GroupPictureGUID { get; set; }
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public ICollection<UserGroup> Members { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
    public GroupSettings Settings { get; set; } = null!;
    public int CreaterId { get; set; }
    public User Creater { get; set; } = null!;
}