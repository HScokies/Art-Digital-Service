using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Configurations
{
    internal class CityConfiguration : IEntityTypeConfiguration<CityDto>
    {
        public void Configure(EntityTypeBuilder<CityDto> builder)
        {
            builder.ToTable("cities");
            builder.HasKey(c => c.id);

            builder.HasData(
                    new CityDto() { id = 1, name = "Челябинск" },
                    new CityDto() { id = 2, name = "Копейск" },
                    new CityDto() { id = 3, name = "Южноуральск" },
                    new CityDto() { id = 4, name = "Миасс" },
                    new CityDto() { id = 5, name = "Златоуст" },
                    new CityDto() { id = 6, name = "Сосновский муниципальный район" }
                );
        }
    }
}
