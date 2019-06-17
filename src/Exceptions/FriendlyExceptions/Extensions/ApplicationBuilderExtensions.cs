using Microsoft.AspNetCore.Builder;

namespace Firepuma.Api.Common.Exceptions.FriendlyExceptions.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseFriendlyExceptions(this IApplicationBuilder appBuilder)
        {
            appBuilder.UseMiddleware<FriendlyExceptionsMiddleware>();
        }
    }
}