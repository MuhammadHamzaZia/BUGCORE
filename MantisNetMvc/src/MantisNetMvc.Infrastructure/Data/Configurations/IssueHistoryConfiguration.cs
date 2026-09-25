using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MantisNetMvc.Core.Entities;

namespace MantisNetMvc.Infrastructure.Data.Configurations;

public class IssueHistoryConfiguration : IEntityTypeConfiguration<IssueHistory>
{
    public void Configure(EntityTypeBuilder<IssueHistory> builder)
    {
        builder.ToTable("MantisIssueHistories");
        builder.HasKey(h => h.Id);

        builder.Property(h => h.FieldName)
            .HasMaxLength(64);

        builder.Property(h => h.OldValue)
            .HasMaxLength(255);

        builder.Property(h => h.NewValue)
            .HasMaxLength(255);

        builder.HasOne(h => h.Issue)
            .WithMany(i => i.History)
            .HasForeignKey(h => h.IssueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(h => h.User)
            .WithMany()
            .HasForeignKey(h => h.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
