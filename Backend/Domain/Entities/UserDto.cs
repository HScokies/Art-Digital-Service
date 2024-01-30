using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class UserDto
    {
        public int id { get; set; }

        public StaffDto? Staff { get; set; }
        public ParticipantDto? Participant { get; set; }

        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string patronymic { get; set; } = string.Empty;

        public string email { get; set; } = null!;
        public string password { get; set; } = null!; // salt + hash

        [JsonIgnore]
        public ICollection<TokenDto> refreshTokens = null!;
    }

}
