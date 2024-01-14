using Domain.Core.Utility;
using Infrastructure.Authentication;
using Infrastructure.Emails;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
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
        public static IServiceCollection AddEmailService(this IServiceCollection services)
        {
            string host = Environment.GetEnvironmentVariable("SMTP_HOST") ?? "smtp.ethereal.email";
            string port_str = Environment.GetEnvironmentVariable("SMTP_PORT")!;
            string login = Environment.GetEnvironmentVariable("SMTP_LOGIN") ?? "earline.howe@ethereal.email";
            string password = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? "7q7CrPCD4n8S1BctY7";

            int port = port_str is null ? 587 : int.Parse(port_str);

            services.AddScoped<IEmailProvider, EmailProvider>(options => new EmailProvider(
                login: login,
                password: password,
                host: host,
                port: port
                ));
            return services;
        }
    }
}
