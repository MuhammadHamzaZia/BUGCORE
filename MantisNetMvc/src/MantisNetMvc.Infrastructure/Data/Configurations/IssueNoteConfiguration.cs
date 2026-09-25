using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MantisNetMvc.Core.Entities;

namespace MantisNetMvc.Infrastructure.Data.Configurations;

public class IssueNoteConfiguration : IEntityTypeConfiguration<IssueNote>
{
    public void Configure(EntityTypeBuilder<IssueNote> builder)
    {
        builder.ToTable("MantisIssueNotes");
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Note)
            .IsRequired();

        builder.HasOne(n => n.Issue)
            .WithMany(i => i.Notes)
            .HasForeignKey(n => n.IssueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(n => n.Reporter)
            .WithMany(u => u.AuthoredNotes)
            .HasForeignKey(n => n.ReporterId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
