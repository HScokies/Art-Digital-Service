using Domain.Entities;

namespace Contracts.Participant
{
    public class GetProfileResponse
    {
        public string firstName { get; set; } = null!;
        public string lastName { get; set; } = null!;
        public string email { get; set; } = null!;
        public bool isAdult { get; set; }

        public Status? status { get; set; } = null!;
        public CaseDto Case { get; set; } = null!;
    }

    public class Status
    {
        public string text { get; set; } = null!;
        public bool download { get; set; }
    }
}
