using System.Threading;
using System.Threading.Tasks;

namespace LibraryAPI.StartupTasks
{
    public interface IStartupTask
    {
        Task ExecuteAsync(CancellationToken cancellationToken = default);
    }
}