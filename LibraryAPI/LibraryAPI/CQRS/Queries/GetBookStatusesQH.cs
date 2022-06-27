using FluentValidation;
using LibraryAPI.Contracts.CQRS.Queries;
using LibraryAPI.Contracts.Dtos;
using LibraryAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.CQRS.Queries
{
    public class GetBookStatusesQV : CQValidator<GetBookStatuses>
    {
        public GetBookStatusesQV(LibraryDbContext dbContext)
        {
            RuleFor(cmd => cmd)
                .MustAsync(async (cmd, ct) =>
                {
                    var bookExists = await dbContext.Books
                        .AnyAsync(b => b.Id == cmd.BookId, ct);
                    return bookExists;
                }).WithErrorCode(GetBookStatuses.ErrorCodes.BookDoesNotExist);
        }
    }

    public class GetBookStatusesQH : IQueryHandler<GetBookStatuses, IEnumerable<BookStatusDto>>
    {
        private readonly LibraryDbContext dbContext;

        public GetBookStatusesQH(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<BookStatusDto>> ExecuteAsync(GetBookStatuses query, LibraryContext context)
        {
            var book = await dbContext.Books
                .AsNoTracking()
                .Where(b => b.Id == query.BookId)
                .Include(b => b.StatusHistories)
                .FirstOrDefaultAsync();

            return book?.StatusHistories
                .Select(sh => new BookStatusDto
                {
                    Id = sh.Id,
                    BookId = query.BookId,
                    ModifiedDate = sh.ModifiedDate,
                    Status = sh.Status
                })
                .OrderBy(sh => sh.ModifiedDate);
        }
    }
}