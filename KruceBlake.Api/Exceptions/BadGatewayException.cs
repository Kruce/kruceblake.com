using System.Net;

namespace KruceBlake.Api.Exceptions
{
    public class BadGatewayException(string message, HttpStatusCode statusCode = HttpStatusCode.BadGateway) : BaseException(message, statusCode)
    {
    }
}