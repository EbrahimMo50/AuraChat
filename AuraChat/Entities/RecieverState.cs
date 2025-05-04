using Microsoft.EntityFrameworkCore;

namespace AuraChat.Entities;

public class RecieverState
{
    public int RecieverId { get; set; }
    [DeleteBehavior(DeleteBehavior.NoAction)]
    public User Reciever { get; set; } = null!;
    public DateTime? IsRecieved { get; set; }
    public DateTime? IsRead { get; set; }
}
