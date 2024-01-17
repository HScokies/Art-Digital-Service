using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly AppDbContext ctx;

        public UserRepository(AppDbContext ctx)
        {
            this.ctx = ctx;
        }

        public async Task<bool> CheckIfTypeExistsAsync(int participantTypeId, CancellationToken cancellationToken = default) => await ctx.types.AnyAsync(t => t.id == participantTypeId, cancellationToken);

        public async Task<bool> CheckIfUserExistsAsync(string email, CancellationToken cancellationToken = default) => await ctx.users.AnyAsync(u => u.email == email, cancellationToken);

        public async Task<ParticipantDto> CreateParticipantAsync(ParticipantDto participant, CancellationToken cancellationToken = default)
        {
            var res = await ctx.participants.AddAsync(participant, cancellationToken);
            await ctx.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<ParticipantDto?> GetParticipantByIdAsync(int id, CancellationToken cancellationToken = default) => await ctx.participants.Include(p => p.User).Include(p => p.Case).FirstOrDefaultAsync(p => p.userId == id, cancellationToken);

        public async Task<UserDto?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default) => await ctx.users.Include(u => u.Staff).Include(u => u.Staff!.Role).Include(u => u.Participant).FirstOrDefaultAsync(u => u.email == email, cancellationToken);

        public async Task<UserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default) => await ctx.users.FirstOrDefaultAsync(u => u.id == id, cancellationToken);

        public async Task<int> SaveAsync(CancellationToken cancellationToken = default) => await ctx.SaveChangesAsync(cancellationToken);
    }
}