namespace AuraChat.Entities;

public class Message
{
    public int Id { get; }
    public User Sender { get; set; } = null!;
    /// <summary>
    /// a represenation to user state on message, made as list since messages can be sent to groups
    /// </summary>
    public List<RecieverState> RecieverState { get; set; } = [];
    public DateTime SentTime { get; set; } = DateTime.UtcNow;
    public string Content { get; set; } = null!;
    public string? MediaGUID { get; set; }
}
