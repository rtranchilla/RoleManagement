using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace RoleManager.DataPersistence;
public sealed class RoleDbContext : DbContext
{
	public RoleDbContext(DbContextOptions<RoleDbContext> options) : base(options) { }

	public DbSet<Member>? Members { get; set; }
	public DbSet<Tree>? Trees { get; set; }
	public DbSet<Role>? Roles { get; set; }
	public DbSet<Node>? Nodes { get; set; }  

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Member>()
					.HasIndex(e => e.UniqueName)
					.IsUnique();
        modelBuilder.Entity<Member>()
                    .HasMany(e => e.Roles)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MemberRole>()
                    .ToTable("MemberRoles")
                    .HasKey(e => new { e.MemberId, e.TreeId });
        modelBuilder.Entity<MemberRole>()
                    .HasOne(e => e.Role)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<MemberRole>()
                    .HasOne<Tree>()
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Tree>()
					.HasIndex(e => e.Name)
					.IsUnique();
        modelBuilder.Entity<Tree>()
                    .HasMany(e => e.RequiredNodes)
                    .WithOne()
                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TreeRequiredNode>()
                    .ToTable("TreeRequiredNodes")
                    .HasKey(e => new { e.TreeId, e.NodeId });

        modelBuilder.Entity<Node>()
					.HasIndex(e => new { e.Name, e.TreeId })
					.IsUnique();
        modelBuilder.Entity<Node>()
                    .HasOne(e => e.Tree)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Node>()
                    .HasMany<TreeRequiredNode>()
                    .WithOne(e => e.Node)
                    .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Node>()
                    .HasMany<RoleRequiredNode>()
                    .WithOne(e => e.Node)
                    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Role>()
					.HasOne(e => e.Tree)
					.WithMany()
					.OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Role>()
					.HasMany(e => e.Nodes)
					.WithOne()
					.OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Role>()
					.HasMany(e => e.RequiredNodes)
					.WithOne()
					.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<RoleNode>()
					.ToTable("RoleNodes")
					.HasKey(e => new { e.RoleId, e.NodeId });
        modelBuilder.Entity<RoleNode>()
                    .HasOne(e => e.Node)
                    .WithMany()
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<RoleRequiredNode>()
                    .ToTable("RoleRequiredNodes")
                    .HasKey(e => new { e.RoleId, e.NodeId });
    }
}
