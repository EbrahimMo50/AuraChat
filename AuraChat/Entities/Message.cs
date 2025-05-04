namespace AuraChat.Entities;

public class Message
{
    public int Id { get; private set; }
    public int SenderId { get; set; }
    public User Sender { get; set; } = null!;
    /// <summary>
    /// a represenation to user state on message, made as list since messages can be sent to groups
    /// </summary>
    public ICollection<RecieverState> RecieverState { get; set; } = [];
    public DateTime SentTime { get; set; } = DateTime.UtcNow;
    public string Content { get; set; } = null!;
    public string? MediaGuid { get; set; }
}
