using LibraryAPI.Domain.Authors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryAPI.DataAccess.Repositories
{
    public class AuthorsRepository : AbstractRepository<Author, Guid>
    {
        public AuthorsRepository(LibraryDbContext dbContext)
            : base(dbContext)
        { }

        public override Task<Author> FindAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return DbSet.AsTracking()
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }
    }
}