using LibraryAPI.Domain.StatusHistories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryAPI.DataAccess.Repositories
{
    public class StatusHistoriesRepository : AbstractRepository<StatusHistory, Guid>
    {
        public StatusHistoriesRepository(LibraryDbContext dbContext)
            : base(dbContext)
        { }

        public override Task<StatusHistory> FindAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return DbSet.AsTracking()
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }
    }
}