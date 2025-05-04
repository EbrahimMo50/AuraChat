using AuraChat.Entities;
using Microsoft.AspNetCore.Authorization;

namespace AuraChat.Policies;

public class GroupAdminRequirment : IAuthorizationRequirement { }

public class GroupAdminHandler(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor) : AuthorizationHandler<GroupAdminRequirment>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupAdminRequirment requirement)
    {
        if (httpContextAccessor.HttpContext == null)
            return Task.CompletedTask;

        var groupId = httpContextAccessor.HttpContext.GetRouteValue("groupId");
        if (groupId == null)
            return Task.CompletedTask;

        var userIdClaim = context.User.FindFirst("Id");
        if (userIdClaim == null)
            return Task.CompletedTask;

        var group = dbContext.Groups.FirstOrDefault(u => u.Id == int.Parse(groupId.ToString()!));
        if (group == null)
            return Task.CompletedTask;

        var member = group.Members.FirstOrDefault(m => m.Id == int.Parse(userIdClaim.Value.ToString()));
        if(member == null)
            return Task.CompletedTask;

        if (member.Role == GroupRole.Admin)
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
