namespace AuraChat.Entities;

public class RecieverState
{
    public User Reciever { get; set; } = null!;
    public DateTime? IsRecieved { get; set; }
    public DateTime? IsRead { get; set; }
}
