using System.Net;

namespace Domain.Core.Primitives
{
    public record Error(HttpStatusCode statusCode, string message);
}
