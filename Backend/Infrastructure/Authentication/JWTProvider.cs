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

        public JWTProvider(string tokenCookieName, SymmetricSecurityKey key, string Issuer, string Audience, int Lifetime)
        {
            this.tokenCookieName = tokenCookieName;
            this.Issuer = Issuer;
            this.Audience = Audience;
            this.Lifetime = Lifetime;
            Credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha384Signature);            
        }
        public void IssueStaffToken(HttpResponse Response, int userId, List<string> permissions)
        {
            IssueStaffToken(Response, userId.ToString(), permissions);
        }

        public void IssueUserToken(HttpResponse Response, int userId, string status)
        {
            IssueUserToken(Response, userId.ToString(), status);
        }
        public void ClearToken(HttpResponse Response)
        {
            Response.Cookies.Append(tokenCookieName, "", new CookieOptions { Expires = DateTime.UtcNow.AddDays(-1) }); 
            Response.Cookies.Delete(this.tokenCookieName); // for some reason this does not work in some browsers :(
        }
        private void CreateAndAppendToken(HttpResponse Response, IEnumerable<Claim> claims)
        {
            CreateToken(claims, out string token, out DateTime expires);
            AppendToken(Response, token, expires);
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
        private void AppendToken(HttpResponse Response, string token, DateTime expires)
        {
            CookieOptions cookieOptions = new()
            {
                HttpOnly = true,
                Expires = expires,
                Secure = true,
                SameSite = SameSiteMode.None
            };
            Response.Cookies.Append(tokenCookieName, token, cookieOptions);
        }

        public void IssueUserToken(HttpResponse Response, string userId, string status)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, status)
            };
            CreateAndAppendToken(Response, claims);
        }

        public void IssueStaffToken(HttpResponse Response, string userId, List<string> permissions)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
            };
            foreach (var permission in permissions)
            {
                claims.Add(new Claim(ClaimTypes.Role, permission.ToString()));
            }
            CreateAndAppendToken(Response, claims);
        }
    }
}
