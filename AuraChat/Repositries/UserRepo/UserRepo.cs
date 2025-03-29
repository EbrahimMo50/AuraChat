using AuraChat.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuraChat.Repositries.UserRepo;

public class UserRepo(AppDbContext dbContext) : IUserRepo
{
    public async Task<User> AddAsync(User user)
    {
        var result = await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

}
