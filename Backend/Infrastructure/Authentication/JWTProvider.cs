using Data.Interfaces;
using Domain.Core.Primitives;
using Domain.Entities;
using Domain.Enumeration;
using Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Authentication
{
    public class JWTProvider : IJWTProvider
    {
        private readonly string accessTokenCookieName;
        private readonly string issuer;
        private readonly string audience;
        private readonly int refreshExpiry; // in hours,
        private readonly int accessExpiry; // in minutes
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
            IHttpContextAccessor httpContextAccessor,
            IRefreshTokenRepository tokenRepository
            )
        {
            this.accessTokenCookieName = accessTokenCookieName;
            this.issuer = issuer;
            this.audience = audience;
            this.refreshExpiry = refreshExpiry;
            this.accessExpiry = accessExpiry;
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

            token = new JwtSecurityTokenHandler().WriteToken(accessToken);
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
            token = new JwtSecurityTokenHandler().WriteToken(refreshToken);
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
            httpContext.Response.Cookies.Append("DEVICE_ID", deviceId, cookieOptions);
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
        public void ClearToken()
        {
            httpContext.Response.Cookies.Append(accessTokenCookieName, "", new CookieOptions { Expires = DateTime.UtcNow.AddDays(-1) });
            httpContext.Response.Cookies.Delete(accessTokenCookieName); // for some reason this does not work in some browsers :(
        }

        public async Task<Result<bool>> TryIssueAccessToken(int userId, string deviceId, CancellationToken cancellationToken)
        {
            var res = await tokenRepository.GetAsync(deviceId, userId, cancellationToken);
            if (res is null) return new Result<bool>(CommonErrors.User.RefreshTokenNotFound);

            var refreshToken = new JwtSecurityTokenHandler().ReadJwtToken(res.token);
            if (refreshToken.ValidTo < DateTime.UtcNow)
            {
                await tokenRepository.DropAsync(res, cancellationToken);
                return new Result<bool>(CommonErrors.User.RefreshTokenExpired);
            }
            var claims = httpContext.User.Claims;
            CreateAndAppendToken(claims);
            return new Result<bool>();
        }
    }
}
