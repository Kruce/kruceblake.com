using System.Net;

namespace KruceBlake.Api.Exceptions
{
    public class InternalServerErrorException(string message) : BaseException(message, HttpStatusCode.Unauthorized)
    {
    }
}
