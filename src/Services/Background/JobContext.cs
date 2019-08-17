using System.Threading;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global

namespace Firepuma.Api.Common.Services.Background
{
    public class JobContext
    {
        public CancellationToken CancellationToken { get; }
        public long IterationNumber { get; }

        public JobContext(CancellationToken cancellationToken, long iterationNumber)
        {
            CancellationToken = cancellationToken;
            IterationNumber = iterationNumber;
        }
    }
}