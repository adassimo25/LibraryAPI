using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryAPI.StartupTasks.Extensions
{
    public static class StartupTaskWebHostExtensions
    {
        public static async Task RunWithTasksAsync(this IHost host, CancellationToken cancellationToken = default)
        {
            await RunStartupTasks(host, cancellationToken);
            await host.RunAsync(cancellationToken);
        }

        private static async Task RunStartupTasks(IHost host, CancellationToken cancellationToken)
        {
            try
            {
                var startupTasks = host.Services.GetServices<IStartupTask>();

                foreach (var startupTask in startupTasks)
                {
                    await startupTask.ExecuteAsync(cancellationToken);
                }
            }
            catch (Exception e)
            {
                var message = $"{e.Message} Stack trace: {e.StackTrace}";
                throw new Exception(message);
            }
        }
    }
}