using System.Net;

namespace KruceBlake.Api.Exceptions
{
    public class BaseException : Exception
    {
        public HttpStatusCode StatusCode { get; } = HttpStatusCode.InternalServerError;

        public TimeSpan? RetryAfter { get; }

        public BaseException() { }

        public BaseException(string message)
            : base(message) { }

        public BaseException(string message, Exception innerException)
            : base(message, innerException) { }

        public BaseException(string message, HttpStatusCode statusCode)
        : this(message)
        {
            StatusCode = statusCode;
        }

        public BaseException(string message, HttpStatusCode statusCode, TimeSpan? retryAfter = null)
        : this(message)
        {
            StatusCode = statusCode;
            RetryAfter = retryAfter;
        }
    }
}