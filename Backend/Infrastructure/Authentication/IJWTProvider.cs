using Domain.Enumeration;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Authentication
{
    public interface IJWTProvider
    {
        public void IssueUserToken(HttpResponse Response, int userId, string status);
        public void IssueStaffToken(HttpResponse Response, int userId, List<string> permissions);
        public void IssueUserToken(HttpResponse Response, string userId, string status);
        public void IssueStaffToken(HttpResponse Response, string userId, List<string> permissions);
        public void ClearToken(HttpResponse Response);

    }
}
