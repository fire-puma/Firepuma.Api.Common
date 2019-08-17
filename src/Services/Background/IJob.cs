using System.Threading.Tasks;

namespace Firepuma.Api.Common.Services.Background
{
    public interface IJob
    {
        Task<string> GetReasonToSkip();

        Task Do(JobContext context);
    }
}
