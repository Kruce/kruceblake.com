using System.Net;

namespace KruceBlake.Api.Exceptions
{
    public class TooManyRequestsException(string message, TimeSpan? retryAfter = null) : BaseException(message, HttpStatusCode.TooManyRequests, retryAfter)
    {
    }
}