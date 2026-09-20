using System.Net;

namespace Freightmatic.Application.Shared
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public string Error { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public ErrorResponse()
        {
            Message = "An internal server error occurred. Please try again later or contact support if the issue persists.";
            Error = "Internal Server Error";
            StatusCode = HttpStatusCode.InternalServerError;
        }

        public ErrorResponse(string message, string error, HttpStatusCode statusCode)
        {
            Message = message;
            Error = error;
            StatusCode = statusCode;
        }
    }
}
