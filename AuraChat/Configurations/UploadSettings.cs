namespace AuraChat.Configurations;

public class UploadSettings
{
    public ICollection<string> AllowedTypes { get; set; } = [];
    public int MaxFileSizeMB { get; set; }
}
