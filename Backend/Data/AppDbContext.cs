using Domain.Entities;
using Domain.Enumeration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public DbSet<UserDataDto> usersData { get; set; }
        
        public DbSet<ParticipantDataDto> participantsData { get; set; }

        public DbSet<StaffRoleDto> roles { get; set; }
        public DbSet<StaffDto> staff { get; set; }

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresEnum<AccessLevels>();
            modelBuilder.HasPostgresEnum<UserTypes>();

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
