namespace AuraChat.Hubs.MainHub;

public interface IMainHub
{
    public Task RecieveMessage();
    public Task UpdateReadState();
}
