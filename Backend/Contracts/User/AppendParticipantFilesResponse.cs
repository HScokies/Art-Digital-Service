using System.Text.Json.Serialization;

namespace Contracts.User
{
    public class AppendParticipantFilesResponse
    {
        [JsonIgnore]
        public int userId { get; set; }
        [JsonIgnore]
        public string status { get; set; } = null!;

        public string consent { get; set; } = null!;
        public string solution { get; set; } = null!;
    }
}
