using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MantisNetMvc.Core.Entities;

namespace MantisNetMvc.Infrastructure.Data.Configurations;

public class ProjectVersionConfiguration : IEntityTypeConfiguration<ProjectVersion>
{
    public void Configure(EntityTypeBuilder<ProjectVersion> builder)
    {
        builder.ToTable("MantisProjectVersions");
        builder.HasKey(pv => pv.Id);

        builder.Property(pv => pv.Version)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(pv => pv.Description)
            .HasMaxLength(500);

        // In MantisBT, version is unique per project (idx_project_version)
        builder.HasIndex(pv => new { pv.ProjectId, pv.Version }).IsUnique();

        builder.HasOne(pv => pv.Project)
            .WithMany(p => p.Versions)
            .HasForeignKey(pv => pv.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
