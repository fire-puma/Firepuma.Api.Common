using System;
using Firepuma.Api.Common.Exceptions.FriendlyExceptions.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Firepuma.Api.Common.Exceptions.FriendlyExceptions.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddFriendlyExceptionsTransforms(this IServiceCollection services,
            Action<TranformOptions> setupAction)
        {
            services.Configure(setupAction);
        }
    }
}