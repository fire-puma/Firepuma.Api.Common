using System;

namespace Firepuma.Api.Common.Exceptions.FriendlyExceptions.Transforms
{
    public interface ITransform
    {
        ResponseData GetResponseData(Exception ex);
        bool CanHandle<T2>(T2 ex) where T2 : Exception;
    }
}