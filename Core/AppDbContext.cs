using Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Core;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Dependency> Dependencies { get; set; } = null!;
    public DbSet<GitRepository> GitRepositories { get; set; } = null!;
    public DbSet<HealthCheck> HealthChecks { get; set; } = null!;
    public DbSet<IssueLog> IssueLogs { get; set; } = null!;
    public DbSet<Profile> Profiles { get; set; } = null!;
    public DbSet<ProjectGroup> ProjectGroups { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<Technology> Technologies { get; set; } = null!;
    public DbSet<VulnerabilityAlert> VulnerabilityAlerts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.HasPostgresExtension("pgcrypto");

        builder.ApplyConfiguration(new GitRepositoryConfiguration());
        builder.ApplyConfiguration(new HealthCheckConfiguration());
        builder.ApplyConfiguration(new IssueLogConfiguration());
        builder.ApplyConfiguration(new ProjectGroupConfiguration());
        builder.ApplyConfiguration(new ProjectConfiguration());
        builder.ApplyConfiguration(new ProfileConfiguration());
        builder.ApplyConfiguration(new VulnerabilityAlertConfiguration());
    } 
}
