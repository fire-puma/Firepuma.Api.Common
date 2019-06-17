using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Firepuma.Api.Common.Middleware
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ExtractKeyCloakRealmRolesMiddleware
    {
        private readonly RequestDelegate _next;
        
        public ExtractKeyCloakRealmRolesMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // ReSharper disable once UnusedMember.Global
        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (!(context.User.Identity is ClaimsIdentity identityClaims))
                {
                    return;
                }
                
                var roles = KeyCloakRolesHelper.GetRolesFromClaims(identityClaims.Claims);
                if (roles == null)
                {
                    return;
                }

                var rolesClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToArray();

                if (context.User == null)
                {
                    context.User = new ClaimsPrincipal(new ClaimsIdentity(rolesClaims));
                }
                else
                {
                    var appIdentity = new ClaimsIdentity(rolesClaims);
                    context.User.AddIdentity(appIdentity);
                }
            }
            finally
            {
                await _next.Invoke(context);
            }
        }
    }
}