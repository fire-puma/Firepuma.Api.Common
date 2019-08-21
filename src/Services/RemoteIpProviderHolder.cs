using Firepuma.Api.Abstractions.Actor;

namespace Firepuma.Api.Common.Services
{
    public class RemoteIpProviderHolder : IRemoteIpProviderHolder
    {
        public RemoteIpProviderHolder(IRemoteIpProvider provider)
        {
            Provider = provider;
        }

        public IRemoteIpProvider Provider { get; set; }
    }
}