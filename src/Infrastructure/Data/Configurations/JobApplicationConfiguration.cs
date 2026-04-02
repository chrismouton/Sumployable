using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sumployable.Domain.Entities;

namespace Sumployable.Infrastructure.Data.Configurations;

public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        builder.ToTable("JobApplication");
        
        builder.HasKey(e => e.Id);

        builder.Property(e => e.RoleName)
            .HasMaxLength(200)
            .IsRequired();
    }
}
