namespace AuraChat.Entities;

public class RecieverState
{
    public User Reciever { get; set; } = null!;
    public bool IsRecieved { get; set; } = false;
    public bool IsRead { get; set; } = false;
}
