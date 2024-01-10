using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserDto>
    {
        public void Configure(EntityTypeBuilder<UserDto> builder)
        {
            builder.ToTable("users");
            builder.HasKey(u => u.id);
            builder.HasIndex(u => u.email).IsUnique();
            
            builder.HasOne(u => u.Staff).WithOne(s => s.User).HasForeignKey<StaffDto>(s => s.userId);
            builder.HasOne(u => u.Participant).WithOne(p => p.User).HasForeignKey<ParticipantDto>(p => p.userId);
        }
    }
}
