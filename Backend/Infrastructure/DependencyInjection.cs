using Data.Interfaces;
using Domain.Core.Utility;
using Infrastructure.Authentication;
using Infrastructure.Emails;
using Infrastructure.Export;
using Infrastructure.Files;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            string tokenCookieName = "ACCESS_TOKEN";

            string JWT_SECRET = Environment.GetEnvironmentVariable("JWT_SECRET") ?? "ZEfYWI3WFxEUuALpMZLXhpHWn/1uxml/Xjm1ybaShZm7T6OD1OYgFSsYcH4JwcJW8uk=";
            string JWT_ISSUER = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "MIDiS";
            string JWT_AUDIENCE = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "olymp.midis.ru";

            string JWT_REFRESH_EXPIRY = Environment.GetEnvironmentVariable("JWT_REFRESH_EXPIRY_HOURS")!;
            int refreshExpiry;
            if (!int.TryParse(JWT_REFRESH_EXPIRY, out refreshExpiry))
            {
                Debug.WriteLine("Could not parse JWT_REFRESH_EXPIRY variable");
                refreshExpiry = 7; // Default JWT_REFRESH_EXPIRY
            }

            string JWT_ACCESS_EXPIRY = Environment.GetEnvironmentVariable("JWT_ACCESS_EXPIRY_MINUTES")!;
            int accessExpiry;
            if (!int.TryParse(JWT_ACCESS_EXPIRY, out accessExpiry))
            {
                Debug.WriteLine("Could not parse JWT_ACCESS_EXPIRY variable");
                accessExpiry = 5; // Default JWT_ACCESS_EXPIRY    
            }  
            
            string JWT_RESET_EXPIRY = Environment.GetEnvironmentVariable("JWT_RESET_EXPIRY_MINUTES")!;
            int resetExpiry;
            if (!int.TryParse(JWT_RESET_EXPIRY, out resetExpiry))
            {
                Debug.WriteLine("Could not parse JWT_RESET_EXPIRY variable");
                resetExpiry = 5; // Default JWT_RESET_EXPIRY
            }

            SymmetricSecurityKey JWT_KEY = new(Encoding.UTF8.GetBytes(JWT_SECRET));

            services.AddTransient<IJWTProvider, JWTProvider>(options =>
            {
                var httpContextAccessor = options.GetRequiredService<IHttpContextAccessor>();
                var tokenRepository = options.GetRequiredService<IRefreshTokenRepository>();
                return new JWTProvider(
                    accessTokenCookieName: tokenCookieName,
                    key: JWT_KEY,
                    issuer: JWT_ISSUER,
                    audience: JWT_AUDIENCE,
                    refreshExpiry: refreshExpiry,
                    accessExpiry: accessExpiry,
                    resetExpiry: resetExpiry,
                    httpContextAccessor: httpContextAccessor,
                    tokenRepository: tokenRepository
                    );
            });

            services.AddAuthentication(
                JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = JWT_ISSUER,
                        ValidAudience = JWT_AUDIENCE,
                        IssuerSigningKey = JWT_KEY,
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
            string SERVER_URL = Environment.GetEnvironmentVariable("BACKEND_URL") ?? "http://localhost:8080";
            string APP_URL = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "http://localhost:5173";
            string SMTP_HOST = Environment.GetEnvironmentVariable("SMTP_HOST") ?? "smtp.ethereal.email";            
            string SMTP_LOGIN = Environment.GetEnvironmentVariable("SMTP_LOGIN") ?? "earline.howe@ethereal.email";
            string SMTP_PASSWORD = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? "7q7CrPCD4n8S1BctY7";
            
            string SMTP_PORT = Environment.GetEnvironmentVariable("SMTP_PORT")!;
            int port;
            if (!int.TryParse(SMTP_PORT, out port))
            {
                Debug.WriteLine("Could not parse SMTP_PORT variable");
                port = 587;
            }

            
            services.AddScoped<IEmailProvider, EmailProvider>(options =>
            {
                var httpContextAccessor = options.GetRequiredService<IHttpContextAccessor>();
                return new EmailProvider(
                    login: SMTP_LOGIN,
                    password: SMTP_PASSWORD,
                    host: SMTP_HOST,
                    port: port,
                    httpContextAccessor: httpContextAccessor,
                    app_url: APP_URL,
                    server_url: SERVER_URL
                    );
            });
            return services;
        }
        public static IServiceCollection AddFilesService(this IServiceCollection services)
        {
            services.AddSingleton<IFilesProvider, FilesProvider>();
            return services;
        }
        public static IServiceCollection AddExportService(this IServiceCollection services)
        {
            services.AddTransient<IExportProvider, ExportProvider>();
            return services;
        }
    }
}
