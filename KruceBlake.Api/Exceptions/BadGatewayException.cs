using System.Net;

namespace KruceBlake.Api.Exceptions
{
    public class BadGatewayException(string message) : BaseException(message, HttpStatusCode.BadGateway)
    {
    }
}