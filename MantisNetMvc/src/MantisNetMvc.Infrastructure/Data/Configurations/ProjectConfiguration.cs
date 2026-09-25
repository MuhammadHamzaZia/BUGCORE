using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MantisNetMvc.Core.Entities;

namespace MantisNetMvc.Infrastructure.Data.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("MantisProjects");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(128);
        builder.Property(p => p.Description).HasMaxLength(1000);
        builder.HasIndex(p => p.Name).IsUnique();
    }
}
