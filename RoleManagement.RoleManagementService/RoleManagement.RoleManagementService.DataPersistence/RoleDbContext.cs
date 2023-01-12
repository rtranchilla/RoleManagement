using Microsoft.EntityFrameworkCore;

namespace RoleManagement.RoleManagementService.DataPersistence;
public sealed class RoleDbContext : DbContext
{
	public RoleDbContext(DbContextOptions<RoleDbContext> options) : base(options) { }

	public DbSet<Member>? Members { get; set; }
	public DbSet<Tree>? Trees { get; set; }
	//public DbSet<Role>? Roles { get; set; }
	public DbSet<Node>? Nodes { get; set; }  

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Member>()
					.HasIndex(e => e.UniqueName)
					.IsUnique();
        modelBuilder.Entity<Tree>()
					.HasIndex(e => e.Name)
					.IsUnique();
        modelBuilder.Entity<Node>()
					.HasIndex(e => new { e.Name, e.TreeId })
					.IsUnique();
    }
}
