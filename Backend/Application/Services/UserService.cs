using Contracts.User;
using Domain.Core.Primitives;
using Domain.Core.Utility;
using Domain.Enumeration;
using Domain.Repositories;
using Domain.Services;
using Bcrypt = BCrypt.Net.BCrypt;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;

        public UserService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task<Result<bool>> CheckIfUserExistsAsync(string email, CancellationToken cancellationToken)
        {
            if (!Ensure.isEmail(email))
                return new Result<bool>(CommonErrors.User.InvalidEmail);
            var isExists = await userRepository.CheckIfExistsAsync(email, cancellationToken);
            return new Result<bool>(isExists);
        }

        public async Task<Result<int>> RegisterUserAsync(RegisterRequest request, CancellationToken cancellationToken)
        {
            if (!Ensure.isEmail(request.email))
                return new Result<int>(CommonErrors.User.InvalidEmail);

            if (!Ensure.isPassword(request.password))
                return new Result<int>(CommonErrors.User.InvalidPassword);

            var userExists = await userRepository.CheckIfExistsAsync(request.email, cancellationToken);
            if (userExists) return new Result<int>(CommonErrors.User.NonUniqueEmail);

            var userType = await userRepository.GetParticipantTypeAsync(request.userType, cancellationToken);
            if (userType is null)
                return new Result<int>(CommonErrors.User.InvalidUserType);

            string hashedPassword = Bcrypt.EnhancedHashPassword(request.password);
            var createdUser = await userRepository.CreateUserAsync(request.email, hashedPassword, cancellationToken);

            var createdParticipant = await userRepository.CreateParticipantAsync(createdUser, userType, cancellationToken);            

            return new Result<int>(createdParticipant.id);
        }
    }
}
