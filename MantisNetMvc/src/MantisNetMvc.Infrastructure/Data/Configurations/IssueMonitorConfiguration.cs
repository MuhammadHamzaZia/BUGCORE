using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MantisNetMvc.Core.Entities;

namespace MantisNetMvc.Infrastructure.Data.Configurations;

public class IssueMonitorConfiguration : IEntityTypeConfiguration<IssueMonitor>
{
    public void Configure(EntityTypeBuilder<IssueMonitor> builder)
    {
        builder.ToTable("MantisIssueMonitors");
        builder.HasKey(im => new { im.IssueId, im.UserId });

        builder.HasOne(im => im.Issue)
            .WithMany(i => i.Monitors)
            .HasForeignKey(im => im.IssueId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(im => im.User)
            .WithMany(u => u.MonitoredIssues)
            .HasForeignKey(im => im.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
