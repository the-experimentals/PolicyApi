using System;
using System.Threading;
using System.Threading.Tasks;

namespace PolicyApi.StartupTasks
{
    public interface IStartupTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}
