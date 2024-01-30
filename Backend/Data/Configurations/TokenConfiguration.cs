using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    internal class TokenConfiguration : IEntityTypeConfiguration<TokenDto>
    {
        public void Configure(EntityTypeBuilder<TokenDto> builder)
        {
            builder.ToTable("refresh_tokens");
            builder.HasKey(t => t.token);
            builder.HasOne(t => t.user).WithMany(u => u.refreshTokens).HasForeignKey(t => t.userId);
        }
    }
}
