using AuraChat.DTOs;
using AuraChat.Models;
using AuraChat.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuraChat.Controllers;
[Route("api/[controller]")]
[ApiController]
public class MessagesController(MediaService mediaService) : ControllerBase
{

    [HttpPost("User/{receiverId}")]
    public async Task<IActionResult> Send(MessageSetDto message, [FromRoute] int receiverId)
    {
        // temporirly made to save media only
        var senderId = Convert.ToInt32(HttpContext.User.FindFirstValue("id"));
        MediaDetailsModel media = new(senderId, receiverId, RecieverType.Chat);
        await mediaService.SaveMedia(message.Files, media);
        return Ok();
        // return CreatedAtAction("somethingIDk","another something response ig");
    }
}
