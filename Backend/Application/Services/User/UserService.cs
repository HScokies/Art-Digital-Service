using Contracts;
using Contracts.User;
using Data.Interfaces;
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
        private readonly IRepository repository;
        private readonly IUserRepository userRepository;
        private readonly ICaseRepository caseRepository;
        private readonly IFilesService filesSevice;
        private readonly IParticipantRepository participantsRepository;

        public UserService(IRepository repository, IUserRepository userRepository, ICaseRepository caseRepository, IFilesService filesSevice, IParticipantRepository participantRepository)
        {
            this.repository = repository;
            this.userRepository = userRepository;
            this.caseRepository = caseRepository;
            this.filesSevice = filesSevice;
            this.participantsRepository = participantRepository;
        }
        private async Task<bool> isValidUserType(RegisterRequest request, CancellationToken cancellationToken) => await userRepository.TypeExistsAsync(request.userType, cancellationToken);
        private async Task<bool> isUserExists(string email, CancellationToken cancellationToken) => await userRepository.UserExistsAsync(email, cancellationToken);
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
            if (!await caseRepository.ExistsAsync(request.caseId, cancellationToken))
                return new Result<PersonalDataAppendResponse>(CommonErrors.Case.NotFound);

            var participant = await userRepository.GetParticipantByIdAsync(userId, cancellationToken);
            if (participant is null)
                return new Result<PersonalDataAppendResponse>(CommonErrors.User.NotFound);
            
            participant.appendPersonalData(request);
            await repository.SaveChangesAsync(cancellationToken);

            var res = participant.toPersonalDataResponse();

            return new Result<PersonalDataAppendResponse>(res);
        }

        public async Task<Result<AppendParticipantFilesResponse>> AppendParticipantFilesAsync(int userId, IFormFile consent, IFormFile solution, CancellationToken cancellationToken)
        {
            var participant = await userRepository.GetParticipantByIdAsync(userId, cancellationToken);
            if (participant is null)
                return new Result<AppendParticipantFilesResponse>(CommonErrors.User.NotFound);

            var validationResult = ValidateParticipantFiles<AppendParticipantFilesResponse>(consent, solution);
            if (!validationResult.isSuccess)
                return validationResult;

           
            var consentFilenameResult = filesSevice.UploadUserFileAsync(consent, cancellationToken);
            var solutionFileNameResult = filesSevice.UploadUserFileAsync(solution, cancellationToken);
            await Task.WhenAll(solutionFileNameResult, consentFilenameResult);
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
            await repository.SaveChangesAsync(cancellationToken);

            return new Result<AppendParticipantFilesResponse>(response);
        }

        public async Task<Result<int>> CreateParticipantAsync(CreateParticipantRequest request, CancellationToken cancellationToken) 
        {
            var typeExists = await userRepository.TypeExistsAsync(request.typeId, cancellationToken);
            if (!typeExists)
                return new Result<int>(CommonErrors.User.InvalidUserType);
            var caseExists = await caseRepository.ExistsAsync(request.caseId, cancellationToken);
            if (!caseExists)
                return new Result<int>(CommonErrors.Case.NotFound);

            //Upload participant files and add their filenames to request 
            if (request.consent is not null && request.solution is not null)
            {
                var validationResult = ValidateParticipantFiles<int>(request.consent, request.solution);
                if (!validationResult.isSuccess)
                    return validationResult;

                var consentFilenameTask = filesSevice.UploadUserFileAsync(request.consent, cancellationToken);
                var solutionFilenameTask = filesSevice.UploadUserFileAsync(request.solution, cancellationToken);
                await Task.WhenAll(consentFilenameTask, solutionFilenameTask);
                var consentRes = consentFilenameTask.Result;
                var solutionRes = solutionFilenameTask.Result;
                if (!consentRes.isSuccess)
                    return new Result<int>(consentRes.error);
                if (!solutionRes.isSuccess)
                    return new Result<int>(solutionRes.error);

                request.consentFilename = consentRes.value;
                request.solutionFilename = solutionRes.value;
            }
            request.password = Bcrypt.EnhancedHashPassword(request.password);

            var Participant = request.toParticipant();
            var res = await userRepository.CreateParticipantAsync(Participant, cancellationToken);
            return new Result<int>(res.id);
        }
        
        private Result<T> ValidateParticipantFiles<T>(IFormFile consent, IFormFile solution)
        {
            if (!Ensure.isValidFileSize(consent.Length) || !Ensure.isValidFileSize(solution.Length))
                return new Result<T>(CommonErrors.File.LargeFile);

            var consentMimeResult = filesSevice.getMimeType(consent);
            if (!consentMimeResult.isSuccess)
                return new Result<T>(consentMimeResult.error);
            if (!Ensure.isValidConsentMimeType(consentMimeResult.value))
                return new Result<T>(CommonErrors.File.UnsupportedMediaType);

            var solutionMimeResult = filesSevice.getMimeType(solution);
            if (!solutionMimeResult.isSuccess)
                return new Result<T>(solutionMimeResult.error);
            if (!Ensure.isValidSolutionMimeType(solutionMimeResult.value))
                return new Result<T>(CommonErrors.File.UnsupportedMediaType);

            return new Result<T>();
        }

        public async Task<ParticipantTypeDto[]> GetParticipantTypesAsync(CancellationToken cancellationToken) => await userRepository.GetParticipantTypes();

        public async Task<GetParticipantResponse> GetParticipants(CancellationToken cancellationToken, int offset, int take, bool participantsOnly,  bool hasScore = true, bool noScore = true, string? search = null, List<int>? excludeType = null, List<int>? excludeCase = null)
        {
            return await participantsRepository.GetParticipants(
                cancellationToken: cancellationToken,
                offset: offset,
                take: take,
                participantsOnly: participantsOnly,
                hasScore: hasScore,
                noScore: noScore,
                search: search,
                excludeType: excludeType,
                excludeCase: excludeCase
                );
        }
    }
}
