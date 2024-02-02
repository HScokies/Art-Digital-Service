using Api.Controllers.Base;
using Api.Controllers.Extensions;
using Application.Services.Participant;
using Contracts.Participant;
using Contracts.User;
using Domain.Enumeration;
using Infrastructure.Authentication;
using Infrastructure.Emails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ParticipantsController : ApiController
    {
        private readonly IParticipantService participantService;
        private readonly IEmailProvider emailProvider;
        private readonly IJWTProvider JWTProvider;

        public ParticipantsController(ILogger<ApiController> logger, IParticipantService participantService, IEmailProvider emailProvider, IJWTProvider JWTProvider) : base(logger)
        {
            this.participantService = participantService;
            this.emailProvider = emailProvider;
            this.JWTProvider = JWTProvider;
        }

        [HttpPost, Authorize(Roles = Roles.Permissions.createUsers)]
        public async Task<IActionResult> CreateParticipant([FromForm] CreateParticipantRequest request, CancellationToken cancellationToken)
        {
            var Result = await participantService.CreateAsync(request, cancellationToken);
            if (!Result.isSuccess)
                return Problem(Result.error);
            return CreatedAtAction(nameof(CreateParticipant), Result.value);
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetParticipantTypes(CancellationToken cancellationToken)
        {
            var res = await participantService.GetParticipantTypesAsync(cancellationToken);
            return Ok(res);
        }

        [HttpGet("statuses"), Authorize(Roles = Roles.Permissions.readUsers)]
        public IActionResult GetParticipantStatuses()
        {
            List<ParticipantStatusResponse> statuses = new()
            {
                new(){name = "Ожидание персональных данных", id = Roles.ParticipantsStatus.justRegistered },
                new(){name = "Ожидание работы", id = Roles.ParticipantsStatus.sentPersonalData },
                new() { name = "На рассмотрении", id = Roles.ParticipantsStatus.awaitingResults },
                new() { name = "Приглашен на второй этап", id = Roles.ParticipantsStatus.invited },
                new() { name = "Выбыл", id = Roles.ParticipantsStatus.droppedOut }
            };
            return Ok(statuses);
        }

        [HttpGet, Authorize(Roles = Roles.Permissions.readUsers)]
        public async Task<IActionResult> GetParticipantsList(CancellationToken cancellationToken, int offset = 0, int take = 5, bool asc = true, bool hasScore = true, bool noScore = true, string? orderBy = null, string? search = null, [FromQuery] int[]? excludeType = null, [FromQuery] int[]? excludeCase = null)
        {
            if (offset < 0 || take < 1)
                return BadRequest();
            //Для большей ясности
            // hasScore = false => исключить из выборки участников с баллом
            // noScore = false => исключить из выборки участников без балла
            // hasScore & noScore => участники как с баллом так и без балла в выборке

            // true => Отображать только участников с отправленным решением, согласием
            var isJudge = User.ShowOnlyParticipants();

            var res = await participantService.GetParticipantsAsync(
                offset: offset,
                take: take,
                asc: asc,
                orderBy: orderBy,
                participantsOnly: isJudge,
                cancellationToken: cancellationToken,
                hasScore: hasScore,
                noScore: noScore,
                search: search,
                excludeType: excludeType,
                excludeCase: excludeCase
                );
            return Ok(res);
        }

        [HttpGet("{participantId:int}"), Authorize(Roles = Roles.Permissions.readUsers)]
        public async Task<IActionResult> GetParticipant(int participantId, CancellationToken cancellationToken)
        {
            var res = await participantService.GetParticipantAsync(participantId, cancellationToken);
            return res.isSuccess ? Ok(res.value) : Problem(res.error);
        }

        [HttpPut("{participantId:int}"), Authorize(Roles = Roles.Permissions.updateUsers)]
        public async Task<IActionResult> UpdateParticipant(int participantId, [FromForm] UpdateParticipantRequest request, CancellationToken cancellationToken)
        {
            var res = await participantService.UpdateParticipantAsync(participantId, request, cancellationToken);
            return res.isSuccess ? NoContent() : Problem(res.error);
        }

        [HttpPatch("{participantId:int}"), Authorize(Roles = Roles.Permissions.rateUsers)]
        public async Task<IActionResult> RateParticipant(int participantId, [FromForm] RateParticipantRequest request, CancellationToken cancellationToken)
        {
            var res = await participantService.RateParticipantAsync(participantId, request, cancellationToken);
            return res.isSuccess ? NoContent() : Problem(res.error);
        }

        [HttpDelete, Authorize(Roles = Roles.Permissions.deleteUsers)]
        public async Task<IActionResult> DropParticipants([FromQuery] int[] id, CancellationToken cancellationToken)
        {
            if (id.Length < 1) return BadRequest();
            await participantService.DropParticipantsAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPatch("append-data"), Authorize(Roles = Roles.ParticipantsStatus.justRegistered)]
        public async Task<IActionResult> AppendPersonalData([FromForm] PersonalDataAppendRequest request, CancellationToken cancellationToken)
        {
            var UserIdResult = User.GetUserId();
            if (!UserIdResult.isSuccess)
                return Problem(UserIdResult.error);

            var res = await participantService.AppendParticipantDataAsync(UserIdResult.value, request, cancellationToken);
            if (!res.isSuccess)
                return Problem(res.error);

            var response = res.value;
            await emailProvider.SendWelcomeEmail(
                new MimeKit.MailboxAddress(
                    name: response.firstName,
                    address: response.email
                    ),
                youtubeId: response.youtubeId,
                cancellationToken: cancellationToken
                );

            JWTProvider.IssueUserToken(response.userId, response.status);
            return Ok(response);
        }

        [HttpPatch("append-files"), Authorize(Roles = Roles.ParticipantsStatus.sentPersonalData)]
        public async Task<IActionResult> AppendFiles(IFormFile conscent, IFormFile solution, CancellationToken cancellationToken)
        {
            var userIdResult = User.GetUserId();
            if (!userIdResult.isSuccess)
                return Problem(userIdResult.error);

            var Result = await participantService.AppendParticipantFilesAsync(userIdResult.value, conscent, solution, cancellationToken);
            if (!Result.isSuccess)
                return Problem(Result.error);

            var res = Result.value;
            JWTProvider.IssueUserToken(res.userId, res.status);
            return Ok(res);
        }

        [HttpGet("export"), Authorize(Roles = Roles.Permissions.readUsers)] // Опциональный параметр id => экспорт только выделенных
        public async Task<IActionResult> ExportParticipants([FromQuery] int[] id, CancellationToken cancellationToken)
        {
            var res = await participantService.ExportParticipantsAsync(id, cancellationToken);
            return File(res.fileStream, res.contentType, $"Участники {DateTime.UtcNow}");
        }
    }
}
