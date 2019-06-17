using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Firepuma.Api.Common.Exceptions.FriendlyExceptions.Transforms
{
    public class TransformsCollectionBuilder : ITransformsMap, ITransformsCollection
    {
        private readonly List<ITransform> _transforms = new List<ITransform>();

        private TransformsCollectionBuilder()
        {
        }

        public ITransform FindTransform(Exception exception)
        {
            var handler = _transforms.FirstOrDefault(x => x.CanHandle(exception));
            return handler;
        }

        public ITransformTo<T> Map<T>() where T : Exception
        {
            var transform = new Transform<T>(this);
            return transform;
        }

        public ITransformTo<Exception> Map(Func<Exception, bool> matching)
        {
            var transform = new Transform<Exception>(this, matching);
            return transform;
        }

        public ITransformTo<Exception> MapAllOthers()
        {
            return Map<Exception>();
        }

        public ITransformsCollection Done()
        {
            return this;
        }

        public static ITransformsMap Begin()
        {
            return new TransformsCollectionBuilder();
        }


        private class Transform<T> : ITransformTo<T>, ITransform where T : Exception
        {
            private readonly Func<Exception, bool> _matcher;
            private readonly TransformsCollectionBuilder _transformsCollectionBuilder;
            private Func<T, ResponseData> _responseGenerator;

            public Transform(TransformsCollectionBuilder transformsCollectionBuilder)
                : this(transformsCollectionBuilder, ex => ex.GetType() == typeof(T))
            {
            }

            public Transform(TransformsCollectionBuilder transformsCollectionBuilder, Func<Exception, bool> matching)
            {
                _transformsCollectionBuilder = transformsCollectionBuilder;
                _matcher = matching;
            }

            public ResponseData GetResponseData(Exception ex2)
            {
                var ex = (T) ex2;
                return _responseGenerator(ex);
            }

            public bool CanHandle<T2>(T2 ex) where T2 : Exception
            {
                var result = _matcher(ex);
                if (!result)
                    result = _matcher(new Exception());
                return result;
            }

            public ITransformsMap To(Func<T, ResponseData> responseGenerator)
            {
                _responseGenerator = responseGenerator;
                _transformsCollectionBuilder._transforms.Add(this);
                return _transformsCollectionBuilder;
            }

            public ITransformsMap To(HttpStatusCode statusCode, string reasonPhrase, Func<T, string> contentGenerator, string contentType = "text/plain")
            {
                return To(ex => new ResponseData(statusCode, reasonPhrase, contentGenerator(ex), contentType));
            }
        }
    }
}