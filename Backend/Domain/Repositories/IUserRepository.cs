using Domain.Core.Primitives;
using Contracts.User;

namespace Domain.Repositories
{
    public interface IUserRepository
    {
        Task<bool> CheckIfExistsAsync(string email);
        Task<Result<bool>> CreateUserAsync(RegisterRequest request);
        
    }
}
