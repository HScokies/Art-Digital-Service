using Domain.Enumeration;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Infrastructure.Authentication
{
    public class JWTProvider : IJWTProvider
    {
        private readonly string tokenCookieName;
        private readonly string Issuer;
        private readonly string Audience;
        private readonly int Lifetime;
        private readonly SigningCredentials Credentials;
        private readonly HttpContext httpContext;

        public JWTProvider(string tokenCookieName, SymmetricSecurityKey key, string Issuer, string Audience, int Lifetime, IHttpContextAccessor httpContextAccessor)
        {
            this.tokenCookieName = tokenCookieName;
            this.Issuer = Issuer;
            this.Audience = Audience;
            this.Lifetime = Lifetime;
            Credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha384Signature);

            this.httpContext = httpContextAccessor.HttpContext!;
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
            httpContext.Response.Cookies.Append(tokenCookieName, "", new CookieOptions { Expires = DateTime.UtcNow.AddDays(-1) });
            httpContext.Response.Cookies.Delete(this.tokenCookieName); // for some reason this does not work in some browsers :(
        }
        private void CreateAndAppendToken(IEnumerable<Claim> claims)
        {
            CreateToken(claims, out string token, out DateTime expires);
            AppendToken(token, expires);
        }
        private void CreateToken(IEnumerable<Claim> claims, out string token, out DateTime expires)
        {
            expires = DateTime.UtcNow.AddHours(Lifetime);
            JwtSecurityToken jwttoken = new(
                    claims: claims,
                    expires: expires,
                    issuer: Issuer,
                    audience: Audience,
                    signingCredentials: Credentials
                );

            token = new JwtSecurityTokenHandler().WriteToken(jwttoken);
        }
        private void AppendToken(string token, DateTime expires)
        {
            CookieOptions cookieOptions = new()
            {
                HttpOnly = true,
                Expires = expires,
                Secure = true,
                SameSite = SameSiteMode.None
            };
            httpContext.Response.Cookies.Append(tokenCookieName, token, cookieOptions);
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
    }
}
