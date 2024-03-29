﻿using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models;

public class Project : BaseModel
{
    public string Name { get; set; } = null!;
    
    [ForeignKey(nameof(ProjectGroup))]
    public Guid GroupId { get; set; }
    public ProjectGroup? Group { get; set; }

    public List<GitRepository> GitRepositories { get; set; } = new List<GitRepository>();
}

public class ProjectConfiguration : BaseModelConfiguration<Project>
{
}
