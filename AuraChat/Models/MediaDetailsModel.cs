namespace AuraChat.Models;

public class MediaDetailsModel
{
    public int SenderId { get; set; }
    public RecieverType RecievrType { get; set; }
    public int RecieverId { get; set; }
    public MediaType MediaType { get; set; }
    public string Guid { get; set; } = null!;
    public string FileName { get; set; } = null!;

    public MediaDetailsModel(int senderId, int recieverId, RecieverType recievrType)
    {
        SenderId = senderId;
        RecieverId = recieverId;
        RecievrType = recievrType;
    }

    public MediaDetailsModel(int senderId, RecieverType recievrType, int recieverId, MediaType mediaType, string guid, string fileName)
    {
        SenderId = senderId;
        RecieverId = recieverId;
        MediaType = mediaType;
        RecievrType = recievrType;
        Guid = guid;
        FileName = fileName;
    }
}

public enum MediaType
{
    Audio, Video ,Image, Application
}

public enum RecieverType
{
    Group, Chat
}