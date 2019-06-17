using System.Threading.Tasks;
using Firepuma.Api.Abstractions.Actor;

namespace Firepuma.Api.Common.Services
{
    public class CachedActorProvider<TActor> : IActorProvider<TActor> where TActor : IActorIdentity
    {
        private readonly IActorProvider<TActor> _provider;

        private TActor _cachedActor;

        public CachedActorProvider(IActorProvider<TActor> provider)
        {
            _provider = provider;
        }

        public async Task<TActor> GetActor()
        {
            if (_cachedActor != null)
            {
                return _cachedActor;
            }

            _cachedActor = await _provider.GetActor();

            return _cachedActor;
        }
    }
}