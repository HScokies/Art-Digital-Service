using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    internal class StaffConfiguration : IEntityTypeConfiguration<StaffDto>
    {
        public void Configure(EntityTypeBuilder<StaffDto> builder)
        {
            builder.ToTable("staff");
            builder.HasKey(s => s.id);

            builder.HasOne(s => s.User).WithOne(s => s.Staff);
            builder.HasOne(s => s.Role).WithMany(r => r.Staff).HasForeignKey(s => s.roleId);

            builder.HasData(
                    new StaffDto()
                    {
                        id = 1,
                        userId = 1,
                        roleId = 1
                    }
                );
        }
    }
}
