using Firepuma.Api.Common.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Firepuma.Api.Common.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseKeycloakRolesMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExtractKeyCloakRealmRolesMiddleware>();
        }
    }
}