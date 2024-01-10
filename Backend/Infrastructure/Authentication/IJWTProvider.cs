using Domain.Enumeration;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Authentication
{
    public interface IJWTProvider
    {
        public void IssueUserToken(HttpResponse Response, int userId, participant_status status);
        public void IssueStaffToken(HttpResponse Response, int userId, List<access_levels> permissions);

    }
}
