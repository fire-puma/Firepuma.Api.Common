using System;
using System.Linq;
using System.Security.Claims;
using Firepuma.Api.Abstractions.Auth;
using Firepuma.Api.Abstractions.Errors;
using Firepuma.Api.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Firepuma.Api.Common.Services
{
    public class ClaimsClientIdProvider : IClientIdProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IErrorReportingService _errorReportingService;
        private readonly IOptions<ClientIdOptions> _options;

        public ClaimsClientIdProvider(
            IHttpContextAccessor httpContextAccessor,
            IErrorReportingService errorReportingService,
            IOptions<ClientIdOptions> options)
        {
            _httpContextAccessor = httpContextAccessor;
            _errorReportingService = errorReportingService;
            _options = options;
        }

        public string ClientId
        {
            get
            {
                var identityClaims = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
                var claims = identityClaims?.Claims.ToArray() ?? new Claim[0];

                var clientId = claims.SingleOrDefault(x => x.Type == _options.Value.ClaimKey)?.Value;

                if (clientId == null)
                {
                    _errorReportingService.CaptureException(new Exception($"Expected {_options.Value.ClaimKey} claim"));
                    throw new ForbiddenException($"Expected {_options.Value.ClaimKey} claim");
                }

                return clientId;
            }
        }
    }
}