﻿
using Core.Features.Projects.Models;
using Core.Features.Users.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Core
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<GitRepository> GitRepositories { get; set; } = null!;
        public DbSet<Profile> Profiles { get; set; } = null!;
        public DbSet<ProjectGroup> ProjectGroups { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Technology> Technologies { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasPostgresExtension("pgcrypto");

            builder.ApplyConfiguration(new GitRepositoryConfiguration());
            builder.ApplyConfiguration(new ProjectGroupConfiguration());
            builder.ApplyConfiguration(new ProjectConfiguration());
            builder.ApplyConfiguration(new ProfileConfiguration());
        } 
    }
}
