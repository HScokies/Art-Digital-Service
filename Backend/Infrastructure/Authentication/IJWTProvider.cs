
using Domain.Core.Primitives;

namespace Infrastructure.Authentication
{
    public interface IJWTProvider
    {
        public void IssueUserToken(int userId, string status);
        public void IssueStaffToken(int userId, List<string> permissions);

        public void IssueUserToken(string userId, string status);
        public void IssueStaffToken(string userId, List<string> permissions);

        public Task IssueRefreshTokenAsync(int userId, CancellationToken cancellationToken);

        public Task<Result<bool>> TryIssueAccessToken(int userId, string deviceId, CancellationToken cancellationToken);

        public void ClearToken();

    }
}
