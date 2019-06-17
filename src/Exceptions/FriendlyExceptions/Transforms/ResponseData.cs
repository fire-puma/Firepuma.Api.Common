using System.Net;

namespace Firepuma.Api.Common.Exceptions.FriendlyExceptions.Transforms
{
    public class ResponseData
    {
        public HttpStatusCode StatusCode { get; }
        public string ReasonPhrase { get; }
        public string Content { get; }
        public string ContentType { get; }

        public ResponseData(HttpStatusCode statusCode, string reasonPhrase, string content, string contentType = "text/plain")
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            Content = content;
            ContentType = contentType;
        }
    }
}