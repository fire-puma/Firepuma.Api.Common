using Firepuma.Api.Abstractions.Actor;

namespace Firepuma.Api.Common.Services
{
    public class ActorProviderHolder<TActor> : IActorProviderHolder<TActor> where TActor : IActorIdentity
    {
        public ActorProviderHolder(IActorProvider<TActor> provider)
        {
            Provider = provider;
        }

        public IActorProvider<TActor> Provider { get; set; }
    }
}
