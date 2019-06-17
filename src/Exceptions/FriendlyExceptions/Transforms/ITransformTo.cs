using System;
using System.Net;

namespace Firepuma.Api.Common.Exceptions.FriendlyExceptions.Transforms
{
    public interface ITransformTo<T>
    {
        ITransformsMap To(Func<T, ResponseData> responseGenerator);

        ITransformsMap To(HttpStatusCode statusCode, string reasonPhrase,
            Func<T, string> contentGenerator, string contentType = "text/plain");
    }
}