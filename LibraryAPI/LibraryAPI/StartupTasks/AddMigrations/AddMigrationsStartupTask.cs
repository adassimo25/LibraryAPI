using LibraryAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryAPI.StartupTasks.Extensions
{
    public class AddMigrationsStartupTask : IStartupTask
    {
        private readonly IServiceProvider serviceProvider;

        public AddMigrationsStartupTask(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task ExecuteAsync(CancellationToken cancellationToken = default)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<LibraryDbContext>()
                ?? throw new NullReferenceException();

            await dbContext.Database.MigrateAsync(cancellationToken: cancellationToken);
        }
    }
}