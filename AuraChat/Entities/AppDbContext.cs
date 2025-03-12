using Microsoft.EntityFrameworkCore;

namespace AuraChat.Entities;

public class AppDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
