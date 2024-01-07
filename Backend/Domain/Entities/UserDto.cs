using Domain.Enumeration;

namespace Domain.Entities
{
    public class UserDto
    {
        public int id { get; set; }

        public UserTypes userType { get; set; }

        public string? refreshToken { get; set; }

        public string email { get; set; } = null!;
        public byte[] hash { get; set; } = null!;
        public byte[] salt { get; set; } = null!;
    }

}
