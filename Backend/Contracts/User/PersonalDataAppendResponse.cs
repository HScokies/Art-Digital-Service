using System.Text.Json.Serialization;

namespace Contracts.User
{
    public sealed class PersonalDataAppendResponse
    {
        public int userId {  get; set; }
        [JsonIgnore]
        public string status { get; set; } = null!;
        public string email { get; set; } = null!;
        public string firstName { get; set; } = null!;        
        public string youtubeId { get; set; } = null!;
    }
}
