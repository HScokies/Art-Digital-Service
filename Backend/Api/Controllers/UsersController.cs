using Api.Controllers.Base;
using Api.Controllers.Extensions;
using Application.Services.User;
using Contracts.User;
using Domain.Entities;
using Domain.Enumeration;
using Infrastructure.Authentication;
using Infrastructure.Emails;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class UsersController : ApiController
    {
        private readonly IUserService userService;
        private readonly IJWTProvider JWTProvider;
        private readonly IEmailProvider emailProvider;

        public UsersController(ILogger<ApiController> logger, IUserService userService, IJWTProvider JWTProvider, IEmailProvider emailProvider) : base(logger)
        {
            this.userService = userService;
            this.JWTProvider = JWTProvider;
            this.emailProvider = emailProvider;
        }



        [HttpGet]
        public async Task<IActionResult> GetUserByEmail(string email, CancellationToken cancellationToken)
        {
            var result = await userService.CheckIfUserExistsAsync(email, cancellationToken);
            return result.isSuccess ? Ok(result.value) : Problem(result.error);
        }

        [HttpPatch("append-data"), Authorize(Roles = Roles.ParticipantsStatus.justRegistered)]
        public async Task<IActionResult> AppendPersonalData([FromForm]PersonalDataAppendRequest request,CancellationToken cancellationToken)
        {
            var UserIdResult = User.GetUserId();
            if (!UserIdResult.isSuccess)
                return Problem(UserIdResult.error);

            var res = await userService.AppendParticipantDataAsync(UserIdResult.value, request, cancellationToken);
            if (!res.isSuccess)
                return Problem(res.error);

            var response = res.value;
            await emailProvider.SendWelcomeEmail(
                new MimeKit.MailboxAddress(
                    name: response.firstName,
                    address: response.email
                    ),
                youtubeId: response.youtubeId,
                continueUrl: $"https://example.com/{Guid.NewGuid()}",
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

            var Result = await userService.AppendParticipantFilesAsync(userIdResult.value,conscent, solution, cancellationToken);
            if (!Result.isSuccess)
                return Problem(Result.error);

            var res = Result.value;
            JWTProvider.IssueUserToken(res.userId, res.status);
            return Ok(res);
        }

        /*  TODO:
         *      Get profile (case, data)
         *      Get full participant data
         *        Update participant 
         *        Download files
         *      Delete participants (int[])
         *      Export participants (int[]?)
         *      Search participants, user-participants w/o solution & files (string search, int[] excludeUserType, int[] excludeCase,bool, hasScore, bool noScore
         *      Participant types
         */
        [HttpPost("participants"), Authorize(Roles = Roles.Permissions.createUsers)]
        public async Task<IActionResult> CreateParticipant([FromForm] CreateParticipantRequest request, CancellationToken cancellationToken)
        {
            var Result = await userService.CreateParticipantAsync(request, cancellationToken);
            if (!Result.isSuccess)
                return Problem(Result.error);
            return CreatedAtAction(nameof(CreateParticipant), Result.value);
        }

        [HttpGet("participants/types")]
        public async Task<IActionResult> GetParticipantTypes(CancellationToken cancellationToken)
        {
            var res = await userService.GetParticipantTypesAsync(cancellationToken);
            return Ok(res);
        }

        [HttpGet("participants/statuses"), Authorize(Roles = Roles.Permissions.readUsers)]
        public IActionResult GetParticipantStatuses()
        {
            Dictionary<string, string> statuses = new()
            {
                {"Ожидание работы", Roles.ParticipantsStatus.sentPersonalData },
                {"На рассмотрении", Roles.ParticipantsStatus.awaitingResults },
                {"Приглашен на второй этап", Roles.ParticipantsStatus.invited},
                {"Выбыл", Roles.ParticipantsStatus.droppedOut }
            };
            return Ok(statuses);
        }


        [HttpGet("participants"), Authorize(Roles = Roles.Permissions.readUsers)]
        public async Task<IActionResult> GetParticipantsList(CancellationToken cancellationToken,[FromQuery] int offset = 0, int take = 5, bool hasScore = true, bool noScore = true, string ? search = null, [FromQuery] List<int>? excludeType = null, [FromQuery] List<int>? excludeCase = null)
        {
            if (offset < 0 || take < 1)
                return BadRequest();
            //Для большей ясности
            // hasScore = false => исключить из выборки участников с баллом
            // noScore = false => исключить из выборки участников без балла
            // hasScore & noScore => участники как с баллом так и без балла в выборке

            // true => Отображать только участников с отправленным решением, согласием
            var isJudge = User.ShowOnlyParticipants();

            var res = await userService.GetParticipantsAsync(
                offset: offset,
                take: take,
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
        

        [HttpGet("participants/{participantId:int}"), Authorize(Roles = Roles.Permissions.readUsers)]
        public async Task<IActionResult> GetParticipant(int participantId, CancellationToken cancellationToken)
        {
            var res = await userService.GetParticipantAsync(participantId, cancellationToken);
            return res.isSuccess? Ok(res.value) : Problem(res.error);
        }

        [HttpPut("participants/{participantId:int}"), Authorize(Roles = Roles.Permissions.updateUsers)]
        public async Task<IActionResult> UpdateParticipant(int participantId, [FromForm] UpdateParticipantRequest request, CancellationToken cancellationToken)
        {
            var res = await userService.UpdateParticipantAsync(participantId, request, cancellationToken);
            return res.isSuccess? NoContent() : Problem(res.error);
        }

        [HttpPatch("participants/{participantId:int}"), Authorize(Roles = Roles.Permissions.rateUsers)]
        public async Task<IActionResult> RateParticipant(int participantId,[FromForm] RateParticipantRequest request, CancellationToken cancellationToken)
        {
            var res = await userService.RateParticipantAsync(participantId, request, cancellationToken);
            return res.isSuccess? NoContent() : Problem(res.error);
        }
    }
}
