namespace Domain.Entities
{
    public class CaseDto
    {
        public int id {  get; set; }
        public ICollection<ParticipantDto> participants { get; set; } = null!;
        public string name { get; set; } = null!;
        public string task { get; set; } = null!;
        public string video { get; set; } = null!; // embed видео ( Мастер-класс )
        public List<string> stages { get; set; } = null!;
        public List<string> criterias { get; set; } = null!;
        
    }
}
