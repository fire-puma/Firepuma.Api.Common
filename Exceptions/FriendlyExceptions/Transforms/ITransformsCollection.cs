using System;

namespace Firepuma.Api.Common.Exceptions.FriendlyExceptions.Transforms
{
    public interface ITransformsCollection
    {
        ITransform FindTransform(Exception exception);
    }
}