using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configurations
{
    internal class TypeConfiguration : IEntityTypeConfiguration<ParticipantTypeDto>
    {
        public void Configure(EntityTypeBuilder<ParticipantTypeDto> builder)
        {
            builder.ToTable("user_types");
            builder.HasKey(t => t.id);

            builder.HasMany(t => t.Participants).WithOne(p => p.Type).HasForeignKey(p => p.typeId);

            builder.HasData(
                    new ParticipantTypeDto()
                    {
                        id = 1,
                        name = "Школьник",
                        isAdult = false
                    },
                    new ParticipantTypeDto()
                    {
                        id = 2,
                        name = "Студент ВО",
                        isAdult = true
                    },
                    new ParticipantTypeDto()
                    {
                        id = 3,
                        name = "Студент СПО",
                        isAdult = true
                    }
                );
        }
    }
}
