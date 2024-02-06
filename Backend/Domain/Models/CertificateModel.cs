namespace Domain.Models
{
    public class CertificateModel
    {
        public double paddingLeft {  get; set; }
        public double paddingTop { get; set; }
        public double paddingRight { get; set; }
        public double paddingBottom { get; set; }
        public string blankFilename { get; set; } = null!;
    }
}
