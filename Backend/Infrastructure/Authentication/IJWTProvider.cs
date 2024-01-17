using Domain.Enumeration;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Infrastructure.Authentication
{
    public interface IJWTProvider
    {
        public void IssueUserToken(int userId, string status);
        public void IssueStaffToken(int userId, List<string> permissions);
        public void IssueUserToken(string userId, string status);
        public void IssueStaffToken(string userId, List<string> permissions);
        public void ClearToken();

    }
}
