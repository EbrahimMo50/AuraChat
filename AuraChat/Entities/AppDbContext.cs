using Microsoft.EntityFrameworkCore;

namespace AuraChat.Entities;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Audit> Audits { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>(entity =>
        {
            entity.OwnsOne(u => u.UserSettings);
        });

        modelBuilder.Entity<Group>(entity =>
        {
            entity.OwnsOne(g => g.Settings);
            entity.HasOne(g => g.Creator).WithOne().HasForeignKey<Group>(g => g.CreatorId);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.OwnsMany(m => m.RecieverState);
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasOne(c => c.UserA).WithOne().HasForeignKey<Chat>(c => c.UserAId);
            entity.HasOne(c => c.UserB).WithOne().HasForeignKey<Chat>(c => c.UserBId);
        });
    }
}
