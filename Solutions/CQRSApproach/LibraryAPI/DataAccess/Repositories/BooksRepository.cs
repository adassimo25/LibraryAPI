using LibraryAPI.Domain.Books;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryAPI.DataAccess.Repositories
{
    public class BooksRepository : AbstractRepository<Book, Guid>
    {
        public BooksRepository(LibraryDbContext dbContext)
            : base(dbContext)
        { }

        public override Task<Book> FindAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return DbSet.AsTracking()
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }
    }
}