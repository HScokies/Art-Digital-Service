using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Contracts.File
{
    public class AppendCertificateRequest
    {
        public double paddingLeft {  get; set; }
        public double paddingTop { get; set; }
        public double paddingRight { get; set; }        
        public double paddingBottom { get; set; }
        public IFormFile? blank {  get; set; } = null;
    }
}
