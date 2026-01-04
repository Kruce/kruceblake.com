using System.Net;

namespace KruceBlake.Api.Exceptions
{
    public class ServiceUnavailableException(string message, TimeSpan? retryAfter = null) : BaseException(message, HttpStatusCode.ServiceUnavailable, retryAfter)
    {
    }
}