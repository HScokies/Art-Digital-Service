using Domain.Enumeration;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Contracts.User;

namespace Data.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly AppDbContext ctx;

        public UserRepository(AppDbContext ctx)
        {
            this.ctx = ctx;
        }
        public async Task<bool> CheckIfExistsAsync(string email, CancellationToken cancellationToken = default) => await ctx.users.AnyAsync(u => u.email == email, cancellationToken);

        public async Task<ParticipantDto> CreateParticipantAsync(UserDto user, ParticipantTypeDto type, CancellationToken cancellationToken = default)
        {
            var participant = new ParticipantDto()
            {
                userId = user.id,            
                User = user,
                typeId = type.id,
                Type = type,
                status = participant_status.Created
            };
            var res = await ctx.participants.AddAsync(participant, cancellationToken);
            await ctx.SaveChangesAsync();

            return res.Entity;
        }

        public async Task<UserDto> CreateUserAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = new UserDto()
            {
                email = email,
                password = password
            };
            var res = await ctx.users.AddAsync(user, cancellationToken);
            await ctx.SaveChangesAsync(cancellationToken);
            
            return res.Entity;
        }

        public async Task<ParticipantTypeDto?> GetParticipantTypeAsync(int participantTypeId, CancellationToken cancellationToken = default) => await ctx.types.FirstOrDefaultAsync(ut => ut.id == participantTypeId, cancellationToken);
    }
}
