using Contracts.User;
using Data.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Data.Repositories
{
    internal class UserRepository : IUserRepository, IParticipantRepository
    {
        private readonly AppDbContext ctx;

        public UserRepository(AppDbContext ctx) => this.ctx = ctx;

        public async Task<bool> TypeExistsAsync(int participantTypeId, CancellationToken cancellationToken = default) => await ctx.types.AnyAsync(t => t.id == participantTypeId, cancellationToken);

        public async Task<bool> UserExistsAsync(string email, CancellationToken cancellationToken = default) => await ctx.users.AnyAsync(u => u.email == email, cancellationToken);

        public async Task<ParticipantDto> CreateParticipantAsync(ParticipantDto participant, CancellationToken cancellationToken = default)
        {
            var res = await ctx.participants.AddAsync(participant, cancellationToken);
            await ctx.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<ParticipantDto?> GetParticipantByUserIdAsync(int id, CancellationToken cancellationToken = default) => await ctx.participants.Include(p => p.User).Include(p => p.Case).FirstOrDefaultAsync(p => p.userId == id, cancellationToken);

        public async Task<ParticipantTypeDto[]> GetParticipantTypes(CancellationToken cancellationToken = default) => await ctx.types.ToArrayAsync(cancellationToken);

        public async Task<UserDto?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default) => await ctx.users.Include(u => u.Staff).Include(u => u.Staff!.Role).Include(u => u.Participant).FirstOrDefaultAsync(u => u.email == email, cancellationToken);

        public async Task<UserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default) => await ctx.users.FirstOrDefaultAsync(u => u.id == id, cancellationToken);

        public async Task<GetParticipantResponse> GetParticipants(CancellationToken cancellationToken, int offset, int take,  bool participantsOnly = false, bool hasScore = true, bool noScore = true, string? search = null, List<int>? excludeType = null, List<int>? excludeCase = null)
        {
            IQueryable<ParticipantDto> query = ctx.participants.Include(p => p.User);
            if (participantsOnly)
                query = query.Where(p => p.solutionFilename != null && p.consentFilename != null);
            if (!hasScore)
                query = query.Where(p => p.rating == null);
            if (!noScore)
                query = query.Where(p => p.rating != null);
            if (!search.IsNullOrEmpty())
                query = query.Where(p => EF.Functions.ILike(p.User.lastName + " " + p.User.firstName + " " + p.User.patronymic, $"%{search}%"));
            if (excludeType is not null)
                query = query.Where(p => !excludeType.Contains(p.typeId));
            if (excludeCase is not null)
                query = query.Where(p => !excludeCase.Contains((int)p.caseId!));

            var participants = query.Select(p => new ParticipantPreview()
            {
                id = p.id,
                fullName = $"{p.User.lastName} {p.User.firstName} {p.User.patronymic}",
                typeName = p.Type.name,
                caseName = p.Case.name,
                score = p.rating
            });
            

            int currentPage = offset > 0? offset / take + 1 : 1; // Запрашиваемая страница
            int pageCount = (int)Math.Ceiling((double)participants.Count() / (double)take); //Всего страниц доступно
            currentPage = currentPage > pageCount ? pageCount : currentPage; // Обновляем текущую страницу если нужно
            var response = new GetParticipantResponse()
            {
                currentPage = currentPage,
                pagesTotal = pageCount
            };
            if (pageCount < 1)
                return response;

            response.participants = await participants.Skip((currentPage-1)*take).Take(take).ToArrayAsync();
            return response;
        }

        public async Task<ParticipantDto?> GetParticipantById(int id, CancellationToken cancellationToken) => await ctx.participants.Include(p => p.User).FirstOrDefaultAsync(p => p.id == id, cancellationToken);
    }
}