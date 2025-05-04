using System.ComponentModel.DataAnnotations;

namespace AuraChat.Entities;

public class Audit
{
    public int Id { get; private set; }
    public string EndPointPath { get; set; } = null!;
    public DateTime AccessTime { get; set; } = DateTime.UtcNow;
    public int RequesterId { get; set; }
}
