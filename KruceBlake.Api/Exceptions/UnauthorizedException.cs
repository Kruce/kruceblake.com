using System.Net;

namespace KruceBlake.Api.Exceptions
{
    public class UnauthorizedException(string message) : BaseException(message, HttpStatusCode.Unauthorized)
    {
    }
}