using System.Net;

namespace KruceBlake.Api.Exceptions
{
    public abstract class BaseException : Exception
    {
        public HttpStatusCode StatusCode { get; } = HttpStatusCode.InternalServerError;

        public TimeSpan? RetryAfter { get; }

        protected BaseException() 
        { 
        }

        protected BaseException(string message)
            : base(message) 
        { 
        }

        protected BaseException(string message, Exception innerException)
            : base(message, innerException) 
        { 
        }

        protected BaseException(string message, HttpStatusCode statusCode)
            : this(message)
        {
            StatusCode = statusCode;
        }

        protected BaseException(string message, HttpStatusCode statusCode, TimeSpan? retryAfter = null)
            : this(message)
        {
            StatusCode = statusCode;
            RetryAfter = retryAfter;
        }
    }
}