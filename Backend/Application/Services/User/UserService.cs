using Contracts;
using Contracts.User;
using Data.Interfaces;
using Domain.Core.Primitives;
using Domain.Core.Utility;
using Domain.Entities;
using Domain.Enumeration;
using Domain.Repositories;
using Infrastructure.Authentication;
using System.IdentityModel.Tokens.Jwt;
using Bcrypt = BCrypt.Net.BCrypt;

namespace Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IParticipantRepository participantRepository;
        private readonly IJWTProvider jwtProvider;
        private readonly IRefreshTokenRepository tokenRepository;

        public UserService(IUserRepository userRepository, IParticipantRepository participantRepository, IJWTProvider jwtProvider, IRefreshTokenRepository tokenRepository)
        {
            this.userRepository = userRepository;
            this.participantRepository = participantRepository;
            this.jwtProvider = jwtProvider;
            this.tokenRepository = tokenRepository;
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

            await jwtProvider.IssueRefreshTokenAsync(createdParticipant.id, cancellationToken);
            jwtProvider.IssueUserToken(createdParticipant.id, Roles.ParticipantsStatus.justRegistered);


            return new Result<int>(createdParticipant.id);
        }

        public async Task<Result<string>> LoginUserAsync(LoginRequest request, CancellationToken cancellationToken)
        {
            if (!Ensure.isEmail(request.email))
                return new Result<string>(CommonErrors.User.InvalidEmail);

            if (!Ensure.isPassword(request.password))
                return new Result<string>(CommonErrors.User.InvalidPassword);

            var res = await userRepository.GetUserByEmailAsync(request.email, cancellationToken);
            if (res is null)
                return new Result<string>(CommonErrors.User.NotFound);

            if (!Bcrypt.EnhancedVerify(request.password, res.password))
                return new Result<string>(CommonErrors.User.InvalidCredentials);

            var userRoleResult =  GetUserRole(res);
            if (userRoleResult.isSuccess) await jwtProvider.IssueRefreshTokenAsync(res.id, cancellationToken); ;

            return userRoleResult;
        }

        private Result<string> GetUserRole(UserDto user)
        {
            if (user.Staff is not null)
            {
                jwtProvider.IssueStaffToken(user.id, user.Staff.Role.PermissionsList);
                return new Result<string>(UserTypes.staff);
            }
            if (user.Participant is not null)
            {
                jwtProvider.IssueUserToken(user.id, user.Participant.status);
                return new Result<string>(user.Participant.status == Roles.ParticipantsStatus.justRegistered ? UserTypes.user : UserTypes.participant);
            }
            return new Result<string>(CommonErrors.Unknown);
        }

        public async Task<Result<bool>> RefreshTokenAsync(int userId, string deviceId, CancellationToken cancellationToken)
        {
            return await jwtProvider.TryIssueAccessToken(userId, deviceId, cancellationToken);
        }
    }
}
