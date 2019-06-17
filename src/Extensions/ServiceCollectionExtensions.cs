using System;
using Firepuma.Api.Abstractions.Actor;
using Firepuma.Api.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Firepuma.Api.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCachedActorProvider<TActor>(
            this IServiceCollection services,
            Func<IServiceProvider, IActorProvider<TActor>> actualActorProviderFactory) where TActor : IActorIdentity
        {
            services.AddScoped<IActorProvider<TActor>>(s => new CachedActorProvider<TActor>(
                actualActorProviderFactory(s)));
        }
    }
}