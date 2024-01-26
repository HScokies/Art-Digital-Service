using System.ComponentModel.DataAnnotations;

namespace Contracts.User
{
    public class RateParticipantRequest
    {
        public int? rating { get; set; } = null!;
        public string status { get; set; } = null!;
    }
}
