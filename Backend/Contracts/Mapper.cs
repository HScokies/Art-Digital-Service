using Contracts.User;
using Domain.Entities;

namespace Contracts
{
    public static class Mapper
    {
        public static ParticipantDto toParticipant(this RegisterRequest request)
        {
            return new ParticipantDto()
            {
                User = new UserDto() { email = request.email, password = request.password },
                typeId = request.userType
            };
        }
    }
}
