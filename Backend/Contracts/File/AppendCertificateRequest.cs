using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;

namespace Contracts.File
{
    public class AppendCertificateRequest
    {
        public float paddingLeft {  get; set; }
        public float paddingTop { get; set; }
        public float paddingRight { get; set; }        
        public float paddingBottom { get; set; }
        public IFormFile? blank {  get; set; } = null;
    }
}
