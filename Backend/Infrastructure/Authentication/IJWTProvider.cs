
using Domain.Core.Primitives;

namespace Infrastructure.Authentication
{
    public interface IJWTProvider
    {
        void IssueUserToken(int userId, string status);
        void IssueStaffToken(int userId, List<string> permissions);

        void IssueUserToken(string userId, string status);
        void IssueStaffToken(string userId, List<string> permissions);

        Task IssueRefreshTokenAsync(int userId, CancellationToken cancellationToken);

        Task<Result<bool>> ValidateRefreshTokenAsync(int userId, string deviceId, CancellationToken cancellationToken);

        string IssuePasswordResetToken();
        bool isTokenActive(string token);
        Task<bool> isValidTokenAsync(string token);

        Task ClearToken(CancellationToken cancellationToken);

    }
}
