using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MantisNetMvc.Core.Entities;

namespace MantisNetMvc.Infrastructure.Data.Configurations;

public class IssueTagConfiguration : IEntityTypeConfiguration<IssueTag>
{
    public void Configure(EntityTypeBuilder<IssueTag> builder)
    {
        builder.ToTable("MantisIssueTags");
        builder.HasKey(it => new { it.IssueId, it.TagId });

        builder.HasOne(it => it.Issue)
            .WithMany(i => i.Tags)
            .HasForeignKey(it => it.IssueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(it => it.Tag)
            .WithMany(t => t.Issues)
            .HasForeignKey(it => it.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
