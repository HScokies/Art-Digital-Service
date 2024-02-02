using System.Text.Json.Serialization;

namespace Contracts.Participant
{
    public sealed class PersonalDataAppendResponse
    {
        public int userId { get; set; }
        [JsonIgnore]
        public string status { get; set; } = null!;
        public string email { get; set; } = null!;
        public string firstName { get; set; } = null!;
        public string youtubeId { get; set; } = null!;
        public bool isAdult { get; set; }
    }
}
