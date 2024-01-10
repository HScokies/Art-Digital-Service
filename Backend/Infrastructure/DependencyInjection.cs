using Domain.Core.Utility;
using Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services)
        {
            string Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "MIDiS";
            string Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "olymp.midis.ru";
            string? strTokenLifetime_H = Environment.GetEnvironmentVariable("JWT_TOKEN_LIFETIME_HOURS");
            int TokenLifetime_H = strTokenLifetime_H is null ? 5 : int.Parse(strTokenLifetime_H);
            string JwtSecret = Environment.GetEnvironmentVariable("JWT_KEY") ?? "ZEfYWI3WFxEUuALpMZLXhpHWn/1uxml/Xjm1ybaShZm7T6OD1OYgFSsYcH4JwcJW8uk=";
            string tokenCookieName = "token";

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(JwtSecret));

            services.AddSingleton<IJWTProvider, JWTProvider>(options => new JWTProvider(tokenCookieName, key, Issuer, Audience, TokenLifetime_H));

            services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Issuer,
                        ValidAudience = Audience,
                        IssuerSigningKey = key,
                        LifetimeValidator = Ensure.isValidExpireTime
                    };
                    options.Events = new()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies[tokenCookieName];
                            return Task.CompletedTask;
                        }
                    };
                    
                });

            return services;
        }
    }
}
