using Microsoft.EntityFrameworkCore;

namespace RoleManagement.RoleManagementService.DataPersistence;
public sealed class RoleDbContext : DbContext
{
	public RoleDbContext(DbContextOptions<RoleDbContext> options) : base(options) { }

	public DbSet<Member>? Members { get; set; }
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Member>()
					.HasIndex(e => e.UniqueName)
					.IsUnique();
	}
}
