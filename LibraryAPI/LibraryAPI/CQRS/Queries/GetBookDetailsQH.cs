using AutoMapper;
using FluentValidation;
using LibraryAPI.Contracts.CQRS.Queries;
using LibraryAPI.Contracts.Dtos;
using LibraryAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.CQRS.Queries
{
    public class GetBookDetailsQV : CQValidator<GetBookDetails>
    {
        public GetBookDetailsQV(LibraryDbContext dbContext)
        {
            RuleFor(cmd => cmd)
                .MustAsync(async (cmd, ct) =>
                {
                    var bookExists = await dbContext.Books
                        .AnyAsync(b => b.Id == cmd.BookId, ct);
                    return bookExists;
                }).WithErrorCode(GetBookDetails.ErrorCodes.BookDoesNotExist);
        }
    }

    public class GetBookDetailsQH : IQueryHandler<GetBookDetails, BookDetailsDto>
    {
        private readonly LibraryDbContext dbContext;
        private readonly IMapper mapper;

        public GetBookDetailsQH(LibraryDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<BookDetailsDto> ExecuteAsync(GetBookDetails query, LibraryContext context)
        {
            var book = await dbContext.Books
                .AsNoTracking()
                .Where(b => b.Id == query.BookId)
                .Include(b => b.Authors)
                .Include(b => b.CurrentStatus)
                .FirstOrDefaultAsync();

            var bookDetails = mapper.Map<BookDetailsDto>(book);

            bookDetails.Author = mapper.Map<AuthorDto>(book.Authors.FirstOrDefault());
            bookDetails.CurrentPrice = -1; // TODO

            return bookDetails;
        }
    }
}