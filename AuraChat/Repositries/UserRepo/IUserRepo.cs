using AuraChat.Entities;

namespace AuraChat.Repositries.UserRepo;

public interface IUserRepo
{
    public Task<User> AddAsync(User user);
    public Task<User?> GetByIdAsync(int id);
    public Task<User?> GetByEmailAsync(string email);
}
