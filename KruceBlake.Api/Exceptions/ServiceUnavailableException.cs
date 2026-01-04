using System.Net;

namespace KruceBlake.Api.Exceptions
{
    public class ServiceUnavailableException(string message) : BaseException(message, HttpStatusCode.ServiceUnavailable)
    {
    }
}