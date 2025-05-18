using System.ComponentModel.DataAnnotations;

namespace AuraChat.DTOs;

public class MessageSetDto
{
    [MinLength(1)]
    public string Content { get; set; } = null!;
    public List<IFormFile> Files { get; set; } = [];
}
