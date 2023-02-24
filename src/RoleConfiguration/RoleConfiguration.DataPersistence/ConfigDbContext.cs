using Microsoft.EntityFrameworkCore;

namespace RoleConfiguration.DataPersistence;

public sealed class ConfigDbContext : DbContext
{
    public ConfigDbContext(DbContextOptions<ConfigDbContext> options) : base(options) { }

    public DbSet<Role>? Roles { get; set; }
    public DbSet<Tree>? Trees { get; set; }

    public DbSet<Member>? Members { get; set; }

    public DbSet<File>? Files { get; set; }
    public DbSet<Source>? Sources { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>()
                    .HasIndex(e => e.UniqueName)
                    .IsUnique();
        modelBuilder.Entity<Tree>()
                    .HasIndex(e => e.Name)
                    .IsUnique();
        modelBuilder.Entity<Role>()
                    .HasIndex(e => new { e.Name, e.TreeId })
                    .IsUnique();

        modelBuilder.Entity<File>()
                    .HasIndex(e => new { e.Path, e.SourceId })
                    .IsUnique();
        modelBuilder.Entity<FileMember>()
                    .ToTable("FileMembers")
                    .HasKey(e => new { e.FileId, e.MemberId });
        modelBuilder.Entity<FileMember>()
                    .HasOne(e => e.Member)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<FileRole>()
                    .ToTable("FileRoles")
                    .HasKey(e => new { e.FileId, e.RoleId });
        modelBuilder.Entity<FileRole>()
                    .HasOne(e => e.Role)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<FileTree>()
                    .ToTable("FileTrees")
                    .HasKey(e => new { e.FileId, e.TreeId });
        modelBuilder.Entity<FileTree>()
                    .HasOne(e => e.Tree)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Source>()
                    .HasIndex(e => e.Name)
                    .IsUnique();
    }
}