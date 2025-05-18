using AuraChat.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace AuraChat.Hubs.MainHub;

[Authorize]
public class MainHub(ILogger<MainHub> logger) : Hub<IMainHub>
{
    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, Context.GetHttpContext()!.User.FindFirstValue("id")!);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.GetHttpContext()!.User.FindFirstValue("id")!;

        if (exception != null)
        {
            logger.LogInformation(exception, "Client {ConnectionId} ( Id {userId} ) disconnected unexpectedly", Context.ConnectionId,userId);
        }
        await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        await base.OnConnectedAsync();
    }
}
