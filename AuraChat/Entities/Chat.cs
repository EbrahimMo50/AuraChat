using Microsoft.EntityFrameworkCore;

namespace AuraChat.Entities;

// this class was made to reduce the search time instead of looking through messages we look at this table if the userA or userB needs messages
/// <summary>
/// this class determines the chats the users had
/// </summary>


[Index(nameof(UserAId), IsUnique = false)]
[Index(nameof(UserBId), IsUnique = false)]

public class Chat
{
    public int Id { get; private set; }
    public int? UserAId { get; set; }                    // will have index
    public User UserA { get; set; } = null!;            
    public int? UserBId { get; set; }                    // will have index
    public User UserB { get; set; } = null!;    
    public ICollection<Message> Messages { get; set; } = [];   // will have pagination on it
}
