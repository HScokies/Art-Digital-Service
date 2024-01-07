namespace Domain.Entities
{
    public class UserDataDto
    {
        public int id { get; set; }
        public int userId { get; set; }
        public UserDto user { get; set; } = null!;
        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string patronymic { get; set; } = null!;
    }
}
