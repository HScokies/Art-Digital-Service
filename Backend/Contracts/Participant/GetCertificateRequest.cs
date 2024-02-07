namespace Contracts.Participant
{
    public class GetCertificateRequest
    {
        public string participantName { get; set; } = null!;
        public string caseName { get; set; } = null!;
    }
}
