using Contracts.User;
using Domain.Core.Primitives;

namespace Domain.Services
{
    public interface IUserService
    {
        Task<Result<bool>> CheckIfUserExistsAsync(string email, CancellationToken cancellationToken);      
        Task<Result<int>> RegisterUserAsync(RegisterRequest request, CancellationToken cancellationToken);
    }
}
