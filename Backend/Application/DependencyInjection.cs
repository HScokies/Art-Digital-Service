using Application.Services.BackgroundWorkers;
using Application.Services.Case;
using Application.Services.Participant;
using Application.Services.Staff;
using Application.Services.User;
using Data.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IParticipantService, ParticipantService>();
            services.AddScoped<IStaffService, StaffService>();
            services.AddScoped<ICaseService, CaseService>();

            return services;
        }

        public static IServiceCollection AddBackgroundWorkers(this IServiceCollection services)
        {
            string JWT_REFRESH_EXPIRY = Environment.GetEnvironmentVariable("JWT_REFRESH_EXPIRY_HOURS")!;
            int refreshExpiry;
            if (!int.TryParse(JWT_REFRESH_EXPIRY, out refreshExpiry))
            {
                Debug.WriteLine("Could not parse JWT_REFRESH_EXPIRY variable");
                refreshExpiry = 7;
            }

            services.AddHostedService<ClearExpiredTokens>(options =>
            {
                var serviceProvider = options.GetRequiredService<IServiceProvider>();
                return new ClearExpiredTokens(refreshExpiry, serviceProvider);
            });

            return services;
        }
    }
}
