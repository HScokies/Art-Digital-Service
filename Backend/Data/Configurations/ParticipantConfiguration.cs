using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    internal class ParticipantConfiguration : IEntityTypeConfiguration<ParticipantDto>
    {
        public void Configure(EntityTypeBuilder<ParticipantDto> builder)
        {
            builder.ToTable("participants");
            builder.HasKey(p => p.id);            

            builder.HasOne(p => p.User).WithOne(u => u.Participant);
            builder.HasOne(p => p.Type).WithMany(t => t.Participants).HasForeignKey(p => p.typeId);
            builder.HasOne(p => p.Case).WithMany(c => c.participants).HasForeignKey(p => p.caseId);        
        }
    }
}
