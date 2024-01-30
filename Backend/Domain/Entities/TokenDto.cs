namespace Domain.Entities
{
    public class TokenDto
    {
        public string token { get; set; } = null!;
        public string deviceId { get; set; } = null!;
        public int userId { get; set; }
        public UserDto user { get; set; } = null!;
        public DateTime expires { get; set; }
    }
}
