using Firepuma.Api.Common.Exceptions.FriendlyExceptions.Transforms;

namespace Firepuma.Api.Common.Exceptions.FriendlyExceptions.Options
{
    public class TranformOptions
    {
        public virtual ITransformsCollection Transforms { get; set; }
    }
}