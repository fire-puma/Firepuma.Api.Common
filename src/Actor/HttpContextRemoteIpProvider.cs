using Firepuma.Api.Abstractions.Actor;
using Microsoft.AspNetCore.Http;

namespace Firepuma.Api.Common.Actor
{
    public class HttpContextRemoteIpProvider : IRemoteIpProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpContextRemoteIpProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetRemoteIp()
        {
            return _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
        }
    }
}
