using AutoMapper;
using LibraryAPI.Contracts.CQRS.Queries;
using LibraryAPI.Contracts.Dtos;
using LibraryAPI.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.CQRS.Queries
{
    public class GetBooksQV : CQValidator<GetBooks>
    {
        public GetBooksQV(LibraryDbContext dbContext)
        {
        }
    }

    public class GetBooksQH : IQueryHandler<GetBooks, IEnumerable<BookDto>>
    {
        private readonly LibraryDbContext dbContext;
        private readonly IMapper mapper;

        public GetBooksQH(LibraryDbContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<BookDto>> ExecuteAsync(GetBooks query, LibraryContext context)
        {
            var booksPaginated = await dbContext.Books
                .AsNoTracking()
                .OrderBy(b => b.Title)
                .Paginated(query)
                .Include(b => b.Authors)
                .Select(b => new
                {
                    b.Id,
                    b.Title,
                    Author = b.Authors.FirstOrDefault()
                })
                .ToListAsync();

            return booksPaginated
                    .Select(b => new BookDto
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Author = mapper.Map<AuthorDto>(b.Author)
                    });
        }
    }
}