using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
// ReSharper disable UnusedMember.Global

namespace Firepuma.Api.Common.Middleware
{
    public class DelayMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IOptions<DelayOptions> _options;

        public DelayMiddleware(
            RequestDelegate next,
            IOptions<DelayOptions> options)
        {
            _next = next;
            _options = options;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!string.Equals(context.Request.Method, "OPTIONS", StringComparison.InvariantCultureIgnoreCase))
            {
                await Task.Delay(_options.Value.Duration);
            }

            await _next.Invoke(context);
        }
    }

    public class DelayOptions
    {
        [Required]
        public TimeSpan Duration { get; set; } = TimeSpan.FromMilliseconds(100);
    }
}