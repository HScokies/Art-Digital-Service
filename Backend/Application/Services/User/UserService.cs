using Contracts;
using Contracts.User;
using Data.Interfaces;
using Domain.Core.Primitives;
using Domain.Core.Utility;
using Domain.Entities;
using Domain.Enumeration;
using Domain.Repositories;
using Infrastructure.Authentication;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Diagnostics;
using Bcrypt = BCrypt.Net.BCrypt;

namespace Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IParticipantRepository participantRepository;
        private readonly IJWTProvider jwtProvider;
        private readonly IRefreshTokenRepository tokenRepository;
        private readonly IRepository repository;

        public UserService(IUserRepository userRepository, IParticipantRepository participantRepository, IJWTProvider jwtProvider, IRefreshTokenRepository tokenRepository, IRepository repository)
        {
            this.userRepository = userRepository;
            this.participantRepository = participantRepository;
            this.jwtProvider = jwtProvider;
            this.tokenRepository = tokenRepository;
            this.repository = repository;
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

            var roleResult =  GetUserRoleAndIssueAccessToken(res);
            if (!roleResult.isSuccess) return roleResult; // Result<Error>

            await jwtProvider.IssueRefreshTokenAsync(res.id, cancellationToken);
            return roleResult;
        }

        private Result<string> GetUserRoleAndIssueAccessToken(UserDto user)
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

        public async Task<Result<string>> RefreshTokenAsync(int userId, string deviceId, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetUserByIdAsync(userId, cancellationToken);
            if (user is null) return new Result<string>(CommonErrors.User.NotFound);
            
            var validateRefreshTokenResult = await jwtProvider.ValidateRefreshTokenAsync(userId, deviceId, cancellationToken);
            if (!validateRefreshTokenResult.isSuccess) return new Result<string>(validateRefreshTokenResult.error);

            return GetUserRoleAndIssueAccessToken(user);
        }

        public async Task<Result<PasswordResetResponse>> RequestPasswordReset(string email, CancellationToken cancellationToken)
        {
            if (!Ensure.isEmail(email))
                return new Result<PasswordResetResponse>(CommonErrors.User.InvalidEmail);

            var user = await userRepository.GetUserByEmailAsync(email, cancellationToken);
            if (user is null)
                return new Result<PasswordResetResponse>(CommonErrors.User.NotFound);

            string reset_token = jwtProvider.IssuePasswordResetToken();
            user.resetToken = reset_token;
            await repository.SaveChangesAsync(cancellationToken);

            PasswordResetResponse response = new()
            {
                name = user.firstName,
                email = user.email,
                token = reset_token
            };
            return new Result<PasswordResetResponse>(response);
        }

        public async Task<Result<bool>> ResetPassword(string token, string password, CancellationToken cancellationToken)
        {
            if (!await jwtProvider.isValidTokenAsync(token))
                return new Result<bool>(CommonErrors.User.InvalidToken);

            if (!jwtProvider.isTokenActive(token))
                return new Result<bool>(CommonErrors.User.ResetTokenExpired);

            if (!Ensure.isPassword(password))
                return new Result<bool>(CommonErrors.User.InvalidPassword);

            var user = await userRepository.GetUserByResetTokenAsync(token, cancellationToken);
            if (user is null)
                return new Result<bool>(CommonErrors.User.NotFound);
            
            password = Bcrypt.EnhancedHashPassword(password);
            user.password = password;
            user.resetToken = null;
            await repository.SaveChangesAsync(cancellationToken);

            return new Result<bool>();
        }
    }
}
