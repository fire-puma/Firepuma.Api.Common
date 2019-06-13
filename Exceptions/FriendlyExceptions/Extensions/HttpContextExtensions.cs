using System;
using System.Text;
using System.Threading.Tasks;
using Firepuma.Api.Common.Exceptions.FriendlyExceptions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Options;

namespace Firepuma.Api.Common.Exceptions.FriendlyExceptions.Extensions
{
    internal static class HttpContextExtensions
    {
        internal static async Task HandleExceptionAsync(this HttpContext context,
            IOptions<TranformOptions> options,
            Exception exception)
        {
            var transformer = options.Value.Transforms?.FindTransform(exception);
            if (transformer == null)
                throw exception;

            var responseData = transformer.GetResponseData(exception);
            var content = responseData.Content;

            context.Response.ContentType = responseData.ContentType;
            context.Response.StatusCode = (int) responseData.StatusCode;
            context.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = responseData.ReasonPhrase;
            context.Response.ContentLength = Encoding.UTF8.GetByteCount(content);
            await context.Response.WriteAsync(content);
        }
    }
}