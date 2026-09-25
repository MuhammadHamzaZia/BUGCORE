using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MantisNetMvc.Core.Entities;

namespace MantisNetMvc.Infrastructure.Data.Configurations;

public class CustomFieldProjectConfiguration : IEntityTypeConfiguration<CustomFieldProject>
{
    public void Configure(EntityTypeBuilder<CustomFieldProject> builder)
    {
        builder.ToTable("MantisCustomFieldProjects");
        builder.HasKey(cfp => new { cfp.CustomFieldId, cfp.ProjectId });

        builder.HasOne(cfp => cfp.CustomField)
            .WithMany(cf => cf.ProjectLinks)
            .HasForeignKey(cfp => cfp.CustomFieldId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cfp => cfp.Project)
            .WithMany(p => p.CustomFields)
            .HasForeignKey(cfp => cfp.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
