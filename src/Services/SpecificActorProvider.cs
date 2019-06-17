using System.Threading.Tasks;
using Firepuma.Api.Abstractions.Actor;

namespace Firepuma.Api.Common.Services
{
    public class SpecificActorProvider<TActor> : IActorProvider<TActor> where TActor : IActorIdentity
    {
        private readonly TActor _actor;

        public SpecificActorProvider(TActor actor)
        {
            _actor = actor;
        }

        public async Task<TActor> GetActor()
        {
            return await Task.FromResult(_actor);
        }
    }
}
