namespace Domain.Entities
{
    public class UserTypeDto
    {
        public int id {  get; set; }
        public string name { get; set; } = null!;
        public bool isAdult { get; set; }
    }
}
