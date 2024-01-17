using Contracts.User;
using Domain.Core.Primitives;
using Domain.Entities;

namespace Application.Services.User
{
    public interface IUserService
    {
        Task<Result<bool>> CheckIfUserExistsAsync(string email, CancellationToken cancellationToken);
        Task<Result<int>> RegisterUserAsync(RegisterRequest request, CancellationToken cancellationToken);        
        Task<Result<UserDto>> LoginUserAsync(LoginRequest request, CancellationToken cancellationToken);
        Task<Result<PersonalDataAppendResponse>> AppendParticipantDataAsync(int userId, PersonalDataAppendRequest request, CancellationToken cancellationToken);
    }
}
