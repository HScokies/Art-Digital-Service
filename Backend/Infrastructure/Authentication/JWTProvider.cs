using Data.Interfaces;
using DocumentFormat.OpenXml.Drawing;
using Domain.Core.Primitives;
using Domain.Core.Utility;
using Domain.Entities;
using Domain.Enumeration;
using Domain.Repositories;
using Irony.Parsing;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading;

namespace Infrastructure.Authentication
{
    public class JWTProvider : IJWTProvider
    {
        private readonly JwtSecurityTokenHandler tokenHandler = new();
        private readonly string accessTokenCookieName;
        private readonly string deviceIdCookieName = "DEVICE_ID";
        private readonly string issuer;
        private readonly string audience;
        private readonly int refreshExpiry; // in hours,
        private readonly int accessExpiry; // in minutes
        private readonly int resetExpiry; // in minutes
        private readonly IRefreshTokenRepository tokenRepository;
        private readonly SigningCredentials Credentials;
        private readonly HttpContext httpContext;

        public JWTProvider(
            string accessTokenCookieName, 
            SymmetricSecurityKey key, 
            string issuer, 
            string audience, 
            int refreshExpiry,
            int accessExpiry,
            int resetExpiry,
            IHttpContextAccessor httpContextAccessor,
            IRefreshTokenRepository tokenRepository
            )
        {
            this.accessTokenCookieName = accessTokenCookieName;
            this.issuer = issuer;
            this.audience = audience;
            this.refreshExpiry = refreshExpiry;
            this.accessExpiry = accessExpiry;
            this.resetExpiry = resetExpiry;
            this.tokenRepository = tokenRepository;
            Credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha384Signature);

            this.httpContext = httpContextAccessor.HttpContext!;
        }

        private void CreateAndAppendToken(IEnumerable<Claim> claims)
        {
            CreateAccessToken(claims, out string token, out DateTime expires);
            AppendAccessToken(token, expires);
        }

        private void CreateAccessToken(IEnumerable<Claim> claims, out string token, out DateTime expires)
        {
            expires = DateTime.UtcNow.AddMinutes(accessExpiry);
            JwtSecurityToken accessToken = new(
                    claims: claims,
                    expires: expires,
                    issuer: issuer,
                    audience: audience,
                    signingCredentials: Credentials
                );

            token = tokenHandler.WriteToken(accessToken);
        }

        private void CreateRefreshToken(out string token)
        {            
            DateTime expires = DateTime.UtcNow.AddHours(refreshExpiry);
            JwtSecurityToken refreshToken = new(
                    expires: expires,
                    issuer: issuer,
                    audience: audience,
                    signingCredentials: Credentials
                );
            token = tokenHandler.WriteToken(refreshToken);
        }

        private void CreateAndAppendDeviceIdCookie(out string deviceId, DateTime expires)
        {
            deviceId = Guid.NewGuid().ToString();            

            CookieOptions cookieOptions = new()
            {
                HttpOnly = true,
                Expires = expires,
                Secure = true,
                SameSite = SameSiteMode.None
            };
            httpContext.Response.Cookies.Append(deviceIdCookieName, deviceId, cookieOptions);
        }

        private void AppendAccessToken(string token, DateTime expires)
        {
            CookieOptions cookieOptions = new()
            {
                Expires = expires,
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            };
            httpContext.Response.Cookies.Append(accessTokenCookieName, token, cookieOptions);
        }

        public bool isTokenActive(string token)
        {
            var jwtToken = tokenHandler.ReadJwtToken(token);
            return jwtToken.ValidTo > DateTime.UtcNow;
        }

        public void IssueUserToken(string userId, string status)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, status)
            };
            CreateAndAppendToken(claims);
        }

        public void IssueStaffToken(string userId, List<string> permissions)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
            };

            foreach (var permission in permissions)
            {
                claims.Add(new Claim(ClaimTypes.Role, permission.ToString()));
            }
            CreateAndAppendToken(claims);
        }

        public async Task IssueRefreshTokenAsync(int userId, CancellationToken cancellationToken)
        {
            DateTime expires = DateTime.UtcNow.AddHours(refreshExpiry);

            CreateAndAppendDeviceIdCookie(out string deviceId, expires); // write to database
            CreateRefreshToken(out string refreshToken);

            var token = new TokenDto()
            {
                deviceId = deviceId,
                token = refreshToken,
                userId = userId,
                expires = expires
            };

            await tokenRepository.CreateAsync(token, cancellationToken);
        }
        
        public void IssueStaffToken(int userId, List<string> permissions)
        {
            IssueStaffToken(userId.ToString(), permissions);
        }

        public void IssueUserToken(int userId, string status)
        {
            IssueUserToken(userId.ToString(), status);
        }
        public async Task ClearToken(CancellationToken cancellationToken)
        {
            string? deviceId = httpContext.Request.Cookies[deviceIdCookieName];
            if (deviceId is not null)
                await tokenRepository.DropAsync(deviceId, cancellationToken);

            var expiredCookieOption = new CookieOptions { Expires = DateTime.UtcNow.AddDays(-1) };
            httpContext.Response.Cookies.Append(accessTokenCookieName, "", expiredCookieOption);
            httpContext.Response.Cookies.Append(deviceIdCookieName, "", expiredCookieOption);

            httpContext.Response.Cookies.Delete(accessTokenCookieName); // for some reason this does not work in some browsers :(
            httpContext.Response.Cookies.Delete(deviceIdCookieName);
        }


        public async Task<Result<string>> TryIssueAccessToken(int userId, string deviceId, CancellationToken cancellationToken)
        {
            var res = await tokenRepository.GetAsync(deviceId, userId, cancellationToken);
            if (res is null) return new Result<string>(CommonErrors.User.RefreshTokenNotFound);

            if (!isTokenActive(res.token))
            {
                await tokenRepository.DropAsync(res, cancellationToken);
                return new Result<string>(CommonErrors.User.RefreshTokenExpired);
            }
            var claims = httpContext.User.Claims;
            CreateAndAppendToken(claims);
            return getStatusFromClaims(claims);
        }
        private Result<string> getStatusFromClaims(IEnumerable<Claim> claims)
        {
            string firstRoleClaim = claims.First(c => c.Type == ClaimTypes.Role).Value;
            if (Ensure.isStatus(firstRoleClaim))
            {
                return new Result<string>(firstRoleClaim == Roles.ParticipantsStatus.justRegistered ? UserTypes.user : UserTypes.participant);
            }

            List<string> staffReadPermissions = new(){ Roles.Permissions.readUsers, Roles.Permissions.readStaff, Roles.Permissions.readCases };
            return claims.Where(c => c.Type == ClaimTypes.Role).Any(c => staffReadPermissions.Contains(c.Value))? 
            new Result<string>(UserTypes.staff) :
            new Result<string>(CommonErrors.User.InvalidToken);
        }

        public string IssuePasswordResetToken()
        {
            var expires = DateTime.UtcNow.AddMinutes(resetExpiry);
            JwtSecurityToken accessToken = new(
                    expires: expires,
                    issuer: issuer,
                    audience: audience,
                    signingCredentials: Credentials
            );

           return tokenHandler.WriteToken(accessToken);
        }

        public async Task<bool> isValidTokenAsync(string token)
        {
            var validationParams = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = Credentials.Key,
                ValidateIssuer = false,
                ValidateAudience = false
            };
            var res = await tokenHandler.ValidateTokenAsync(token, validationParams);
            return res.IsValid;
        }
    }
}
