using Microsoft.EntityFrameworkCore;
using TeamMeetingRemoteSupport.Models;

namespace TeamMeetingRemoteSupport.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<UserAccount> Users => Set<UserAccount>();
    public DbSet<MeetingRoom> Rooms => Set<MeetingRoom>();
    public DbSet<RoomMember> RoomMembers => Set<RoomMember>();
    public DbSet<ControlGrant> ControlGrants => Set<ControlGrant>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasIndex(u => u.Username).IsUnique();
            entity.Property(u => u.Username).HasMaxLength(100);
        });

        modelBuilder.Entity<MeetingRoom>(entity =>
        {
            entity.HasIndex(r => r.RoomCode).IsUnique();
            entity.Property(r => r.RoomCode).HasMaxLength(50);
            entity.Property(r => r.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<RoomMember>(entity =>
        {
            entity.HasIndex(rm => new { rm.RoomId, rm.UserId }).IsUnique();
            entity.Property(rm => rm.ConnectionId).HasMaxLength(200);
        });

        modelBuilder.Entity<ControlGrant>(entity =>
        {
            entity.Property(g => g.RoomCode).HasMaxLength(50);
            entity.Property(g => g.RequesterConnectionId).HasMaxLength(200);
            entity.Property(g => g.TargetConnectionId).HasMaxLength(200);
        });
    }
}
