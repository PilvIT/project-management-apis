using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Core;


/// <summary>
/// BaseModel for Entity Framework Core models, provides Id generation in database level.
/// </summary>
[ExcludeFromCodeCoverage]
public class BaseModel
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid Id { get; set; }
}

[ExcludeFromCodeCoverage]
public class BaseModelConfiguration<TModel> : IEntityTypeConfiguration<TModel> where TModel : BaseModel
{
    public void Configure(EntityTypeBuilder<TModel> builder)
    {
        builder
            .Property(model => model.Id)
            .HasDefaultValueSql("gen_random_uuid()");
    }
}
