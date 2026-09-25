using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MantisNetMvc.Core.Entities;

namespace MantisNetMvc.Infrastructure.Data.Configurations;

public class ProjectHierarchyConfiguration : IEntityTypeConfiguration<ProjectHierarchy>
{
    public void Configure(EntityTypeBuilder<ProjectHierarchy> builder)
    {
        builder.ToTable("MantisProjectHierarchies");
        builder.HasKey(ph => new { ph.ParentProjectId, ph.ChildProjectId });

        builder.Ignore(ph => ph.InheritParent);

        builder.HasOne(ph => ph.ParentProject)
            .WithMany(p => p.Subprojects)
            .HasForeignKey(ph => ph.ParentProjectId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ph => ph.ChildProject)
            .WithMany(p => p.ParentProjects)
            .HasForeignKey(ph => ph.ChildProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
