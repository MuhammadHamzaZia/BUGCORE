using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MantisNetMvc.Core.Entities;

namespace MantisNetMvc.Infrastructure.Data.Configurations;

public class IssueRelationshipConfiguration : IEntityTypeConfiguration<IssueRelationship>
{
    public void Configure(EntityTypeBuilder<IssueRelationship> builder)
    {
        builder.ToTable("MantisIssueRelationships");
        builder.HasKey(ir => ir.Id);

        builder.HasOne(ir => ir.SourceIssue)
            .WithMany(i => i.SourceRelationships)
            .HasForeignKey(ir => ir.SourceIssueId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ir => ir.DestinationIssue)
            .WithMany(i => i.DestinationRelationships)
            .HasForeignKey(ir => ir.DestinationIssueId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
