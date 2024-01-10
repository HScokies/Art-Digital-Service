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
    internal class CaseConfiguration : IEntityTypeConfiguration<CaseDto>
    {
        public void Configure(EntityTypeBuilder<CaseDto> builder)
        {
            builder.ToTable("cases");
            builder.HasKey(c => c.id);            
        }
    }
}
