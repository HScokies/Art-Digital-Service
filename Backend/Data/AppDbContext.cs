using Domain.Core.Primitives;
using Domain.Entities;
using Domain.Enumeration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;

namespace Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            try
            {
                var DbCreator = Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (DbCreator != null)
                {
                    if (!DbCreator.CanConnect()) DbCreator.Create();
                    if (!DbCreator.HasTables()) DbCreator.CreateTables();
                }
            }
            catch(Exception ex)
            {
               Debug.WriteLine($"Database context error!", ex);
            }
        }


        public DbSet<CityDto> cities { get; set; }
        
        public DbSet<CaseDto> cases { get; set; }
        
        public DbSet<UserDto> users { get; set; }
        public DbSet<StaffDto> staff { get; set; }
        public DbSet<ParticipantDto> participants { get; set; }

        public DbSet<ParticipantTypeDto> types { get; set; }
        public DbSet<StaffRoleDto> roles { get; set; }   

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);            
            modelBuilder.HasPostgresEnum<participant_status>();
            modelBuilder.HasPostgresEnum<access_levels>();
        }
    }
}
