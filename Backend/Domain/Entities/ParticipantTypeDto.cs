using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class ParticipantTypeDto
    {
        public int id {  get; set; }
        public string name { get; set; } = null!;
        public bool isAdult { get; set; }
        [JsonIgnore]
        public ICollection<ParticipantDto> Participants { get; set; } = null!;
    }
}
