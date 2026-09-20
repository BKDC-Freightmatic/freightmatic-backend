using System.Net;

namespace Freightmatic.Application.Shared
{
    public class CustomException : Exception
    {
        public string Error { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public CustomException(string message, string error = "Bad Request", HttpStatusCode statusCode = HttpStatusCode.BadRequest)
            : base(message) // Call the base Exception class constructor
        {
            Error = error;
            StatusCode = statusCode;
        }
    }
}
