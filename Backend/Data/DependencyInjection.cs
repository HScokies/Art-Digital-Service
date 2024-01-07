using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDataLayer(this IServiceCollection services)
        {
            var PG_HOST = Environment.GetEnvironmentVariable("PG_HOST") ?? "localhost";
            var PG_PORT = Environment.GetEnvironmentVariable("PG_PORT") ?? "5432";
            var PG_DATABASE = Environment.GetEnvironmentVariable("PG_DATABASE") ?? "postgres";
            var PG_USER = Environment.GetEnvironmentVariable("PG_USER") ?? "postgres";
            var PG_PASSWORD = Environment.GetEnvironmentVariable("PG_PASSWORD") ?? string.Empty;            

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
    }
}
