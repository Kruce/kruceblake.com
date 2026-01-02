using System.Net;

namespace KruceBlake.Api.Exceptions
{
    public class ServiceUnavailableException(string message, HttpStatusCode statusCode = HttpStatusCode.ServiceUnavailable) : BaseException(message, statusCode)
    {
    }
}