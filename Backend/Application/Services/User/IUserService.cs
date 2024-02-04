using Contracts.User;
using Domain.Core.Primitives;

namespace Application.Services.User
{
    public interface IUserService
    {
        Task<Result<bool>> CheckIfUserExistsAsync(string email, CancellationToken cancellationToken);
        Task<Result<int>> RegisterUserAsync(RegisterRequest request, CancellationToken cancellationToken);        
        Task<Result<string>> LoginUserAsync(LoginRequest request, CancellationToken cancellationToken);
        Task<Result<string>> RefreshTokenAsync(int userId, string deviceId, CancellationToken cancellationToken);
        Task<Result<PasswordResetResponse>> RequestPasswordReset(string email, CancellationToken cancellationToken);
        Task<Result<bool>> ResetPassword(string token, string password, CancellationToken cancellationToken);        
    }
}
