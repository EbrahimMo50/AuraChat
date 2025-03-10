namespace AuraChat.Entities;

// this class was made to reduce the search time instead of looking through messages we look at this table if the userA or userB needs messages
/// <summary>
/// this class determines the chats the users had
/// </summary>

public class Chat
{
    public int Id { get; set; }
    public int UserAId { get; set; }                    // will have index
    public User UserA { get; set; } = null!;            // arguably removable wont be used or very rarely used
    public int UserBId { get; set; }                    // will have index
    public User UserB { get; set; } = null!;    
    public List<Message> Messages { get; set; } = [];   // will have pagination on it
}
