using Bcrypt = BCrypt.Net.BCrypt;
using Contracts;
using Contracts.File;
using Contracts.User;
using Data.Interfaces;
using Domain.Core.Primitives;
using Domain.Core.Utility;
using Domain.Entities;
using Domain.Enumeration;
using Domain.Repositories;
using Infrastructure.Files;
using Microsoft.AspNetCore.Http;
using Contracts.Participant;
using Infrastructure.Export;

namespace Application.Services.Participant
{
    internal class ParticipantService : IParticipantService
    {
        private readonly IFilesProvider filesProvider;
        private readonly IParticipantRepository participantRepository;
        private readonly ICaseRepository caseRepository;
        private readonly IUserRepository userRepository;
        private readonly IRepository repository;
        private readonly IExportProvider exportProvider;

        public ParticipantService(IFilesProvider filesProvider, IParticipantRepository participantRepository, ICaseRepository caseRepository, IUserRepository userRepository, IRepository repository, IExportProvider exportProvider)
        {
            this.filesProvider = filesProvider;
            this.participantRepository = participantRepository;
            this.caseRepository = caseRepository;
            this.userRepository = userRepository;
            this.repository = repository;
            this.exportProvider = exportProvider;
        }

        private async Task<Result<bool>> TryUpdateParticipant(ParticipantDto participant, UpdateParticipantRequest request, CancellationToken cancellationToken, string? oldSolutionFilename, string? oldConsentFilename)
        {
            try
            {
                participant.updateParticipant(request);
                await repository.SaveChangesAsync(cancellationToken);

                //Удаляем старые файлы в случае успеха
                if (oldConsentFilename != null)
                    filesProvider.DropUserFile(oldConsentFilename);
                if (oldSolutionFilename != null)
                    filesProvider.DropUserFile(oldSolutionFilename);

                return new Result<bool>();
            }
            catch
            {
                //Удаляем новые файлы в случае провала
                if (oldConsentFilename != null)
                    filesProvider.DropUserFile(participant.consentFilename!);
                if (oldSolutionFilename != null)
                    filesProvider.DropUserFile(participant.solutionFilename!);

                return new Result<bool>(CommonErrors.Unknown);
            }
        }

        private Result<T> ValidateConsent<T>(IFormFile consent)
        {
            if (!Ensure.isValidFileSize(consent.Length))
                return new Result<T>(CommonErrors.File.LargeFile);

            var consentMimeResult = filesProvider.getMimeType(consent);

            if (!consentMimeResult.isSuccess)
                return new Result<T>(consentMimeResult.error);

            if (!Ensure.isValidConsentMimeType(consentMimeResult.value))
                return new Result<T>(CommonErrors.File.UnsupportedMediaType);

            return new Result<T>();
        }

        private Result<T> ValidateSolutuion<T>(IFormFile solution)
        {
            if (!Ensure.isValidFileSize(solution.Length))
                return new Result<T>(CommonErrors.File.LargeFile);

            var solutionMimeResult = filesProvider.getMimeType(solution);

            if (!solutionMimeResult.isSuccess)
                return new Result<T>(solutionMimeResult.error);

            if (!Ensure.isValidSolutionMimeType(solutionMimeResult.value))
                return new Result<T>(CommonErrors.File.UnsupportedMediaType);

            return new Result<T>();
        }

        private Result<T> ValidateParticipantFiles<T>(IFormFile consent, IFormFile solution)
        {
            var consentResult = ValidateConsent<T>(consent);
            if (!consentResult.isSuccess)
                return consentResult;

            var solutionResult = ValidateSolutuion<T>(solution);
            if (!solutionResult.isSuccess)
                return solutionResult;

            return new Result<T>();
        }

        public Task<Result<FileResponse>> GetParticipantFileAsync(string filename, CancellationToken cancellationToken) => filesProvider.DownloadUserFileAsync(filename, cancellationToken);

        public async Task<Result<PersonalDataAppendResponse>> AppendParticipantDataAsync(int userId, PersonalDataAppendRequest request, CancellationToken cancellationToken)
        {
            if (!Ensure.isPhone(request.phone))
                return new Result<PersonalDataAppendResponse>(CommonErrors.User.InvalidPhone);

            var _case = await caseRepository.GetAsync(request.caseId, cancellationToken);
            if (_case is null)
                return new Result<PersonalDataAppendResponse>(CommonErrors.Case.NotFound);

            var participant = await userRepository.GetParticipantByUserIdAsync(userId, cancellationToken);
            if (participant is null)
                return new Result<PersonalDataAppendResponse>(CommonErrors.User.NotFound);

            participant.appendPersonalData(request);
            await repository.SaveChangesAsync(cancellationToken);
            participant.Case = _case;

            var res = participant.toPersonalDataResponse();

            return new Result<PersonalDataAppendResponse>(res);
        }

        public async Task<Result<AppendParticipantFilesResponse>> AppendParticipantFilesAsync(int userId, IFormFile consent, IFormFile solution, CancellationToken cancellationToken)
        {
            var participant = await userRepository.GetParticipantByUserIdAsync(userId, cancellationToken);
            if (participant is null)
                return new Result<AppendParticipantFilesResponse>(CommonErrors.User.NotFound);

            var validationResult = ValidateParticipantFiles<AppendParticipantFilesResponse>(consent, solution);
            if (!validationResult.isSuccess)
                return validationResult;


            var consentFilenameResult = filesProvider.UploadUserFileAsync(consent, cancellationToken);
            var solutionFileNameResult = filesProvider.UploadUserFileAsync(solution, cancellationToken);
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

        public async Task<Result<int>> CreateAsync(CreateParticipantRequest request, CancellationToken cancellationToken)
        {
            if (!Ensure.isPhone(request.phone))
                return new Result<int>(CommonErrors.User.InvalidPhone);
            if (!Ensure.isPassword(request.password))
                return new Result<int>(CommonErrors.User.InvalidPassword);
            if (!Ensure.isEmail(request.email))
                return new Result<int>(CommonErrors.User.InvalidEmail);

            var userExists = await userRepository.ExistsAsync(request.email, cancellationToken);
            if (userExists)
                return new Result<int>(CommonErrors.User.NonUniqueEmail);

            var typeExists = await participantRepository.TypeExistsAsync(request.typeId, cancellationToken);
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

                var consentFilenameTask = filesProvider.UploadUserFileAsync(request.consent, cancellationToken);
                var solutionFilenameTask = filesProvider.UploadUserFileAsync(request.solution, cancellationToken);
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
            var res = await participantRepository.CreateAsync(Participant, cancellationToken);
            return new Result<int>(res.id);
        }

        public async Task<ParticipantTypeDto[]> GetParticipantTypesAsync(CancellationToken cancellationToken) => await participantRepository.GetTypesAsync(cancellationToken);

        public async Task<GetParticipantsResponse> GetParticipantsAsync(
            CancellationToken cancellationToken, 
            int offset, int take, 
            bool participantsOnly, 
            bool asc = true, string? orderBy = null,
            bool hasScore = true, bool noScore = true, 
            string? search = null, 
            int[]? excludeType = null, 
            int[]? excludeCase = null) => await participantRepository.GetAsync(
                cancellationToken: cancellationToken,
                offset: offset,
                take: take,
                asc: asc,
                orderBy: orderBy,
                participantsOnly: participantsOnly,
                hasScore: hasScore,
                noScore: noScore,
                search: search,
                excludeType: excludeType,
                excludeCase: excludeCase
            );

        public async Task<Result<GetParticipantResponse>> GetParticipantAsync(int userId, CancellationToken cancellationToken)
        {
            var res = await participantRepository.GetAsync(userId, cancellationToken);
            if (res is null)
                return new Result<GetParticipantResponse>(CommonErrors.User.NotFound);
            return new Result<GetParticipantResponse>(res.toParticipantResponse());
        }

        public async Task<Result<bool>> UpdateParticipantAsync(int participantId, UpdateParticipantRequest request, CancellationToken cancellationToken)
        {
            if (!Ensure.isEmail(request.email))
                return new Result<bool>(CommonErrors.User.InvalidEmail);
            if (!Ensure.isPhone(request.phone))
                return new Result<bool>(CommonErrors.User.InvalidPhone);
            if (!Ensure.isStatus(request.status))
                return new Result<bool>(CommonErrors.User.InvalidStatus);

            var participant = await participantRepository.GetAsync(participantId, cancellationToken);
            if (participant is null)
                return new Result<bool>(CommonErrors.User.NotFound);

            string? oldConsentFilename = null, oldSolutionFilename = null;
            if (request.consent is not null)
            {
                var validationResilt = ValidateConsent<bool>(request.consent);
                if (!validationResilt.isSuccess)
                    return validationResilt;

                oldConsentFilename = participant.consentFilename;

                var consentResult = await filesProvider.UploadUserFileAsync(request.consent, cancellationToken);
                if (!consentResult.isSuccess)
                    return new Result<bool>(consentResult.error);

                participant.consentFilename = consentResult.value;
            }
            if (request.solution is not null)
            {
                var validationResilt = ValidateSolutuion<bool>(request.solution);
                if (!validationResilt.isSuccess)
                    return validationResilt;

                oldSolutionFilename = participant.solutionFilename;

                var solutionResult = await filesProvider.UploadUserFileAsync(request.solution, cancellationToken);
                if (!solutionResult.isSuccess)
                    return new Result<bool>(solutionResult.error);
                participant.solutionFilename = solutionResult.value;
            }

            return await TryUpdateParticipant(participant, request, cancellationToken, oldSolutionFilename, oldConsentFilename);
        }

        public async Task<Result<bool>> RateParticipantAsync(int participantId, RateParticipantRequest request, CancellationToken cancellationToken)
        {
            if (!Ensure.isStatus(request.status))
                return new Result<bool>(CommonErrors.User.InvalidStatus);

            var participant = await participantRepository.GetAsync(participantId, cancellationToken);
            if (participant is null)
                return new Result<bool>(CommonErrors.User.NotFound);

            participant.rating = request.rating;
            participant.status = request.status;

            await repository.SaveChangesAsync(cancellationToken);
            return new Result<bool>();
        }

        public async Task DropParticipantsAsync(int[] participantIds, CancellationToken cancellationToken)
        {
            var participants = await participantRepository.GetAsync(participantIds, cancellationToken);
            
            await userRepository.DropAsync(participants.Select(p => p.User), cancellationToken);

            foreach (var participant in participants)
            {
                if (participant.consentFilename != null)
                    filesProvider.DropUserFile(participant.consentFilename);
                if (participant.solutionFilename != null)
                    filesProvider.DropUserFile(participant.solutionFilename);
            }
        }

        public async Task<FileResponse> ExportParticipantsAsync(int[]? participants, CancellationToken cancellationToken)
        {
            ParticipantExportModel[] participantList = await participantRepository.GetExportModelsAsync(participants, cancellationToken);
            return exportProvider.ExportParticipants(participantList);
        }

        public async Task<bool> isAdultAsync(int userId, CancellationToken cancellationToken) => await participantRepository.isAdult(userId, cancellationToken);

        public async Task<Result<GetProfileResponse>> GetProfileAsync(int userId, CancellationToken cancellationToken)
        {
            var res = await participantRepository.GetProfileAsync(userId,cancellationToken);
            if (res is null) return new Result<GetProfileResponse>(CommonErrors.User.NotFound);
            res.status = res.status?.text switch
            {
                Roles.ParticipantsStatus.awaitingResults => new Status() { text = "Ваша заявки принята и находится на рассмотрении.\nОжидайте результатов!", download = false},
                Roles.ParticipantsStatus.invited => new Status() { text = "Поздравляем, вы приглашены на второй этап!", download = true },
                Roles.ParticipantsStatus.droppedOut => new Status() { text = "К сожалению, ваша работа не прошла на второй этап, но вы можете скачать сертификат участника", download = true },
                _ => null
            };
            return new Result<GetProfileResponse>(res);
        }

        public async Task<Result<FileResponse>> GetCertificateAsync(int userId, CancellationToken cancellationToken)
        {
            var CertificateDataResult = await participantRepository.GetCertificateContentAsync(userId, cancellationToken);
            if (CertificateDataResult is null)
                return new Result<FileResponse>(CommonErrors.User.NotFound);

            return await filesProvider.DownloadCertificateAsync(CertificateDataResult, cancellationToken);
        }
    }
}
