namespace Domain.Entities
{
    public class CaseDto
    {
        public int id {  get; set; }
        public ICollection<ParticipantDto> participants { get; set; } = null!;
        public string name { get; set; } = null!;
        public string task { get; set; } = null!;
        public string youtubeId { get; set; } = null!; // https://www.youtube.com/watch?v=dQw4w9WgXcQ => id = dQw4w9WgXcQ
        public List<string> stages { get; set; } = null!;
        public List<string> criterias { get; set; } = null!;
        
    }
}
