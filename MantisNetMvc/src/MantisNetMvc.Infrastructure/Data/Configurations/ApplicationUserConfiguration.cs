using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MantisNetMvc.Core.Entities;

namespace MantisNetMvc.Infrastructure.Data.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.RealName)
            .HasMaxLength(191);

        builder.Property(u => u.UserName)
            .HasMaxLength(191);

        builder.Property(u => u.Email)
            .HasMaxLength(191);
    }
}
