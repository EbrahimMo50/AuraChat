using AuraChat.Entities;

namespace AuraChat.Services;

public static class DbInitializer
{
    public static void Seed(AppDbContext dbContext)
    {
        if (dbContext.Users.Any() && dbContext.Groups.Any() && dbContext.Chats.Any() && dbContext.Messages.Any())
            return;

        // Seed Users
        var user1 = new User { Name = "Alice", Email = "alice@example.com", Salt = "blah", HashedPassword = "blah" };
        var user2 = new User { Name = "Bob", Email = "bob@example.com", Salt = "blah", HashedPassword = "blah" };
        var user3 = new User { Name = "Charlie", Email = "charlie@example.com", Salt = "blah", HashedPassword = "blah" };

        dbContext.Users.AddRange(user1, user2, user3);
        dbContext.SaveChanges();

        // Seed Groups
        var group1 = new Group { Name = "Developers", CreatorId = 1 };
        group1.Members.Add(new UserGroup() { GroupId = group1.Id, Role = GroupRole.Admin, UserId = 1});
        group1.Members.Add(new UserGroup() { GroupId = group1.Id, Role = GroupRole.Member, UserId = 2 });

        var group2 = new Group { Name = "Marketing", CreatorId = 2 };
        group1.Members.Add(new UserGroup() { GroupId = group1.Id, Role = GroupRole.Admin, UserId = 3 });
        group1.Members.Add(new UserGroup() { GroupId = group1.Id, Role = GroupRole.Member, UserId = 2 });

        dbContext.Groups.AddRange(group1, group2);
        dbContext.SaveChanges();

        // Seed Chats
        var chat1 = new Chat { UserAId = 1, UserBId = 2 };
        chat1.Messages.Add(new Message() { Content = "Hello", SenderId = 1 });

        var chat2 = new Chat { UserAId = 2, UserBId = 3 };
        chat1.Messages.Add(new Message() { Content = "Hello World", SenderId = 2 });

        dbContext.Chats.AddRange(chat1, chat2);
        dbContext.SaveChanges();

    }
}
