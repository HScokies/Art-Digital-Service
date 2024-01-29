using Contracts;
using Contracts.User;
using Data.Interfaces;
using Domain.Core.Primitives;
using Domain.Core.Utility;
using Domain.Entities;
using Domain.Enumeration;
using Domain.Repositories;
using Infrastructure.Files;
using Bcrypt = BCrypt.Net.BCrypt;

namespace Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IParticipantRepository participantRepository;

        public UserService(IUserRepository userRepository, IParticipantRepository participantRepository)
        {
            this.userRepository = userRepository;
            this.participantRepository = participantRepository;
        }
        
        private async Task<bool> isValidUserType(RegisterRequest request, CancellationToken cancellationToken) => await participantRepository.TypeExistsAsync(request.userType, cancellationToken);
        
        private async Task<bool> isUserExists(string email, CancellationToken cancellationToken) => await userRepository.ExistsAsync(email, cancellationToken);
        
        public async Task<Result<bool>> CheckIfUserExistsAsync(string email, CancellationToken cancellationToken)
        {
            if (!Ensure.isEmail(email))
                return new Result<bool>(CommonErrors.User.InvalidEmail);

            return new Result<bool>(await isUserExists(email, cancellationToken));
        }

        public async Task<Result<int>> RegisterUserAsync(RegisterRequest request, CancellationToken cancellationToken)
        {
            if (!Ensure.isEmail(request.email))
                return new Result<int>(CommonErrors.User.InvalidEmail);

            if (!Ensure.isPassword(request.password))
                return new Result<int>(CommonErrors.User.InvalidPassword);

            if (await isUserExists(request.email, cancellationToken))
                return new Result<int>(CommonErrors.User.NonUniqueEmail);

            if (!await isValidUserType(request, cancellationToken))
                return new Result<int>(CommonErrors.User.InvalidUserType);

            request.password = Bcrypt.EnhancedHashPassword(request.password);
            var ParticipantModel = request.toParticipant();

            var createdParticipant = await participantRepository.CreateAsync(ParticipantModel, cancellationToken);

            return new Result<int>(createdParticipant.id);
        }

        public async Task<Result<UserDto>> LoginUserAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            if (!Ensure.isEmail(request.email))
                return new Result<UserDto>(CommonErrors.User.InvalidEmail);

            if (!Ensure.isPassword(request.password))
                return new Result<UserDto>(CommonErrors.User.InvalidPassword);

            var res = await userRepository.GetUserByEmailAsync(request.email, cancellationToken);
            if (res is null)
                return new Result<UserDto>(CommonErrors.User.NotFound);

            if (!Bcrypt.EnhancedVerify(request.password, res.password))
                return new Result<UserDto>(CommonErrors.User.InvalidCredentials);

            return new Result<UserDto>(res);
        }
    }
}
