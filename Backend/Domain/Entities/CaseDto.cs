using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class CaseDto
    {
        public int id {  get; set; }        
        public string name { get; set; } = null!;
        public string task { get; set; } = null!;
        public string youtubeId { get; set; } = null!; // https://www.youtube.com/watch?v=dQw4w9WgXcQ => id = dQw4w9WgXcQ
        public string[] stages { get; set; } = null!;
        public string[] criterias { get; set; } = null!;

        [JsonIgnore]
        public ICollection<ParticipantDto> participants { get; set; } = null!;
    }
}
