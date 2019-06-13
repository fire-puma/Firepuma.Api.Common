using System;

namespace Firepuma.Api.Common.Exceptions.FriendlyExceptions.Transforms
{
    public interface ITransformsMap
    {
        ITransformTo<T> Map<T>() where T : Exception;
        ITransformTo<Exception> Map(Func<Exception, bool> matcher);
        ITransformTo<Exception> MapAllOthers();
        ITransformsCollection Done();
    }
}