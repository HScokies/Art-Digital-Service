using Data.Interfaces;
using Data.Repositories;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services)
        {
            var PG_HOST = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
            var PG_PORT = Environment.GetEnvironmentVariable("POSTGRES_PORT") ?? "5432";
            var PG_DATABASE = Environment.GetEnvironmentVariable("POSTGRES_EFCORE_DB") ?? "devdb";
            var PG_USER = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "SU";
            var PG_PASSWORD = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "30LNJUUp4P";            

            services.AddDbContext<AppDbContext>(options =>       
                options.UseNpgsql($"""
                    Server =  {PG_HOST};
                    Port = {PG_PORT};
                    Database = {PG_DATABASE};
                    User Id = {PG_USER};
                    Password = {PG_PASSWORD};
                    """)
            );
            
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IParticipantRepository, ParticipantRepository>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            services.AddScoped<ICaseRepository, CaseRepository>();
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IRepository, Repository>();
            
            return services;
        }
    }
}
