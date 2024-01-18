using Contracts;
using Contracts.User;
using Domain.Core.Primitives;
using Domain.Core.Utility;
using Domain.Entities;
using Domain.Enumeration;
using Domain.Repositories;
using Infrastructure.Files;
using Microsoft.AspNetCore.Http;
using Bcrypt = BCrypt.Net.BCrypt;

namespace Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly ICaseRepository caseRepository;
        private readonly IFilesService filesSevice;

        public UserService(IUserRepository userRepository, ICaseRepository caseRepository, IFilesService filesSevice)
        {
            this.userRepository = userRepository;
            this.caseRepository = caseRepository;
            this.filesSevice = filesSevice;
        }
        private async Task<bool> isValidUserType(RegisterRequest request, CancellationToken cancellationToken) => await userRepository.CheckIfTypeExistsAsync(request.userType, cancellationToken);
        private async Task<bool> isUserExists(string email, CancellationToken cancellationToken) => await userRepository.CheckIfUserExistsAsync(email, cancellationToken);
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

            var createdParticipant = await userRepository.CreateParticipantAsync(ParticipantModel, cancellationToken);

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

        public async Task<Result<PersonalDataAppendResponse>> AppendParticipantDataAsync(int userId, PersonalDataAppendRequest request, CancellationToken cancellationToken)
        {
            if (!await caseRepository.ExistsAsync(request.caseId))
                return new Result<PersonalDataAppendResponse>(CommonErrors.Case.NotFound);

            var participant = await userRepository.GetParticipantByIdAsync(userId, cancellationToken);
            if (participant is null)
                return new Result<PersonalDataAppendResponse>(CommonErrors.User.NotFound);
            
            participant.appendPersonalData(request);
            await userRepository.SaveAsync();

            var res = participant.toPersonalDataResponse();

            return new Result<PersonalDataAppendResponse>(res);
        }

        public async Task<Result<AppendParticipantFilesResponse>> AppendParticipantFilesAsync(int userId, IFormFile consent, IFormFile solution)
        {
            var participant = await userRepository.GetParticipantByIdAsync(userId);
            if (participant is null)
                return new Result<AppendParticipantFilesResponse>(CommonErrors.User.NotFound);

            if (!Ensure.isValidFileSize(consent.Length) || !Ensure.isValidFileSize(solution.Length))
                return new Result<AppendParticipantFilesResponse>(CommonErrors.File.LargeFile);
            
            var consentMimeResult = filesSevice.getMimeType(consent);
            if (!consentMimeResult.isSuccess)
                return new Result<AppendParticipantFilesResponse>(consentMimeResult.error);
            if (!Ensure.isValidConsentMimeType(consentMimeResult.value))
                return new Result<AppendParticipantFilesResponse>(CommonErrors.File.UnsupportedMediaType);

            var solutionMimeResult = filesSevice.getMimeType(solution);
            if (!solutionMimeResult.isSuccess)
                return new Result<AppendParticipantFilesResponse>(solutionMimeResult.error);
            if (!Ensure.isValidSolutionMimeType(solutionMimeResult.value))
                return new Result<AppendParticipantFilesResponse>(CommonErrors.File.UnsupportedMediaType);

            var consentFilenameResult = filesSevice.UploadFileAsync(consent);
            var solutionFileNameResult = filesSevice.UploadFileAsync(solution);
            await consentFilenameResult; 
            await solutionFileNameResult;
            if (!consentFilenameResult.Result.isSuccess)
                return new Result<AppendParticipantFilesResponse>(consentFilenameResult.Result.error);
            if (!solutionFileNameResult.Result.isSuccess)
                return new Result<AppendParticipantFilesResponse>(solutionFileNameResult.Result.error);

            var response = new AppendParticipantFilesResponse()
            {
                status = Roles.ParticipantsStatus.awaitingResults,
                consent = consentFilenameResult.Result.value,
                solution = solutionFileNameResult.Result.value
            };

            participant.consentFilename = response.consent;
            participant.solutionFilename = response.solution;
            participant.status = response.status;
            await userRepository.SaveAsync();

            return new Result<AppendParticipantFilesResponse>(response);
        }
    }
}
