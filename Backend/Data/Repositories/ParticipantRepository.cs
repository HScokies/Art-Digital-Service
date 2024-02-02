using Contracts.Participant;
using Contracts.User;
using Data.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Data.Repositories
{
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly AppDbContext ctx;

        public ParticipantRepository(AppDbContext ctx) => this.ctx = ctx;

        public async Task<bool> TypeExistsAsync(int participantTypeId, CancellationToken cancellationToken = default) => await ctx.types.AnyAsync(t => t.id == participantTypeId, cancellationToken);
        
        public async Task<ParticipantDto> CreateAsync(ParticipantDto participant, CancellationToken cancellationToken = default)
        {
            var res = await ctx.participants.AddAsync(participant, cancellationToken);
            await ctx.SaveChangesAsync();
            return res.Entity;
        }
        
        public async Task<ParticipantTypeDto[]> GetTypesAsync(CancellationToken cancellationToken = default) => await ctx.types.ToArrayAsync(cancellationToken);

        public async Task<GetParticipantsResponse> GetAsync(CancellationToken cancellationToken, int offset, int take, bool asc = true, string? orderBy = null, bool participantsOnly = false, bool hasScore = true, bool noScore = true, string? search = null, int[]? excludeType = null, int[]? excludeCase = null)
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
            if (excludeType?.Length > 0)
                query = query.Where(p => !excludeType.Contains(p.typeId));
            if (excludeCase?.Length > 0)
                query = query.Where(p => !excludeCase.Contains((int)p.caseId!));

            switch (orderBy)
            {
                case "name":
                    query = asc ? query.OrderBy(p => p.User.lastName + " " + p.User.firstName + " " + p.User.patronymic) : query.OrderByDescending(p => p.User.lastName + " " + p.User.firstName + " " + p.User.patronymic);
                    break;
                case "type":
                    query = asc ? query.OrderBy(p => p.Type.name) : query.OrderByDescending(p => p.Type.name);
                    break;
                case "case":
                    query = asc ? query.OrderBy(p => p.Case.name) : query.OrderByDescending(p => p.Case.name);
                    break;
                case "rating":
                    query = asc ? query.OrderBy(p => p.rating) : query.OrderByDescending(p => p.rating);
                    break;
            }

            var participants = query.Select(p => new ParticipantPreview()
            {
                id = p.id,
                fullName = $"{p.User.lastName} {p.User.firstName} {p.User.patronymic}",
                typeName = p.Type.name,
                caseName = p.Case.name,
                rating = p.rating
            });


            int currentPage = offset > 0 ? offset / take + 1 : 1; // Запрашиваемая страница
            int pageCount = (int)Math.Ceiling((double)participants.Count() / take); //Всего страниц доступно
            currentPage = currentPage > pageCount ? pageCount : currentPage; // Обновляем текущую страницу если нужно
            var response = new GetParticipantsResponse()
            {
                currentPage = currentPage,
                pagesTotal = pageCount
            };
            if (pageCount < 1)
                return response;

            response.rows = await participants.Skip((currentPage - 1) * take).Take(take).ToArrayAsync(cancellationToken);
            return response;
        }

        public async Task<ParticipantDto?> GetAsync(int id, CancellationToken cancellationToken) => await ctx.participants.Include(p => p.User).FirstOrDefaultAsync(p => p.id == id, cancellationToken);

        public async Task<ParticipantDto[]> GetAsync(int[]? id, CancellationToken cancellationToken)
        {
            var participants = ctx.participants.Include(p => p.User).AsQueryable();
            if (id?.Length > 0)
                participants = participants.Where(p => id.Contains(p.id));
            return await participants.ToArrayAsync(cancellationToken);
        }

        public async Task DropAsync(ParticipantDto[] participants, CancellationToken cancellationToken)
        {
            ctx.participants.RemoveRange(participants);
            await ctx.SaveChangesAsync(cancellationToken);
        }

        public async Task<ParticipantExportModel[]> GetExportModelsAsync(int[]? id, CancellationToken cancellationToken)
        {
            var participants = ctx.participants.AsQueryable();

            if (id?.Length > 0)
                participants = participants.Where(p => id.Contains(p.id));
            return await participants.Select(p => new ParticipantExportModel()
            {
                firstName = p.User.firstName,
                lastName = p.User.lastName,
                patronymic = p.User.patronymic,
                email = p.User.email,
                userType = p.Type.name,
                caseName = p.Case.name,
                rating = p.rating,
                city = p.city,
                phone = p.phone,
                institution = p.institution,
                speciality = p.speciality,
                grade = p.grade
            }).ToArrayAsync(cancellationToken);
        }

        public async Task<bool> isAdult(int userId, CancellationToken cancellationToken) => await ctx.participants.Where(p => p.userId == userId).Select(p => p.Type.isAdult).FirstAsync(cancellationToken);
    }
}
