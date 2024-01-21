using Application.Services.Participant;
using Application.Services.User;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IParticipantService, ParticipantService>();

            return services;
        }
    }
}
