using AuraChat.Entities;
using Microsoft.AspNetCore.Authorization;

namespace AuraChat.Policies;

public class GroupMemberRequirment : IAuthorizationRequirement { }

public class GroupMemberRequirmentHandler(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<GroupMemberRequirment>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupMemberRequirment requirement)
    {
        if(httpContextAccessor.HttpContext == null)
            return Task.CompletedTask;

        var groupId = httpContextAccessor.HttpContext.GetRouteValue("groupId");
        if(groupId == null)
            return Task.CompletedTask;

        var userIdClaim = context.User.FindFirst("Id");
        if(userIdClaim == null)
            return Task.CompletedTask;

        var user = dbContext.Users.FirstOrDefault(u => u.Id == int.Parse(userIdClaim.Value));
        if(user == null) 
            return Task.CompletedTask;

        if (user.Groups.Any(g => g.Id == int.Parse(groupId.ToString()!)))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}