namespace AuraChat.Entities;

public class Audit
{
    public int Id { get; }
    public string EndPointPath { get; set; } = null!;
    public DateTime AccessTime { get; } = DateTime.UtcNow;
    public int RequesterId { get; set; }
}
