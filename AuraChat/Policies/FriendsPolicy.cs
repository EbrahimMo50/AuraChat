using AuraChat.Entities;
using Microsoft.AspNetCore.Authorization;

namespace AuraChat.Policies;

public class FriendsRequirment : IAuthorizationRequirement { }

public class FriendsHandler(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<FriendsRequirment>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, FriendsRequirment requirement)
    {
        if (httpContextAccessor.HttpContext == null)
            return Task.CompletedTask;

        var friendId = httpContextAccessor.HttpContext.GetRouteValue("friendId");
        if (friendId == null)
            return Task.CompletedTask;

        var userIdClaim = context.User.FindFirst("Id");
        if (userIdClaim == null)
            return Task.CompletedTask;

        var user = dbContext.Users.FirstOrDefault(u => u.Id == int.Parse(userIdClaim.Value));
        if (user == null)
            return Task.CompletedTask;

        if (user.Friends.Any(u => u.Id == int.Parse(friendId.ToString()!)))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
