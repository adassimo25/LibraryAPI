using AutoMapper;
using LibraryAPI.Contracts.Dtos;
using LibraryAPI.DataAccess;
using LibraryAPI.DataAccess.Repositories;
using LibraryAPI.Domain.Authors;
using LibraryAPI.Domain.Books;
using LibraryAPI.Domain.StatusHistories;
using LibraryAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Services
{
    public class BooksService : IBooksService
    {
        private readonly IRepository<Book, Guid> books;
        private readonly IRepository<Author, Guid> authors;
        private readonly IRepository<StatusHistory, Guid> statusHistories;
        private readonly IExternalAPIService<BookPrice> externalAPIBookPriceService;
        private readonly LibraryDbContext dbContext;
        private readonly IMapper mapper;
        private readonly ILogger<BooksService> logger;

        public BooksService(IRepository<Book, Guid> books, IRepository<Author, Guid> authors, IRepository<StatusHistory, Guid> statusHistories,
            IExternalAPIService<BookPrice> externalAPIBookPriceService, LibraryDbContext dbContext,
            IMapper mapper, ILogger<BooksService> logger)
        {
            this.books = books;
            this.authors = authors;
            this.statusHistories = statusHistories;
            this.externalAPIBookPriceService = externalAPIBookPriceService;
            this.dbContext = dbContext;
            this.mapper = mapper;
            this.logger = logger;
        }

        public IEnumerable<BookDto> GetBooks(int page, int limit)
        {
            var paginatedBooksQuery = new PaginatedBooksQuery
            {
                PageNumber = page,
                PageSize = limit
            };

            var booksPaginated = dbContext.Books
                .AsNoTracking()
                .OrderBy(b => b.Title)
                .Paginated(paginatedBooksQuery)
                .Include(b => b.Authors)
                .Select(b => new
                {
                    b.Id,
                    b.Title,
                    Author = b.Authors.FirstOrDefault()
                })
                .ToList();

            return booksPaginated
                    .Select(b => new BookDto
                    {
                        Id = b.Id,
                        Title = b.Title,
                        Author = mapper.Map<AuthorDto>(b.Author)
                    });
        }

        public BookDetailsDto GetBookDetails(Guid bookId)
        {
            var book = dbContext.Books
                .AsNoTracking()
                .Where(b => b.Id == bookId)
                .Include(b => b.Authors)
                .Include(b => b.CurrentStatus)
                .FirstOrDefault();

            var bookDetails = mapper.Map<BookDetailsDto>(book);

            bookDetails.Author = mapper.Map<AuthorDto>(source: book.Authors.FirstOrDefault());

            BookPrice bookPrice = null;
            Task.Run(async () => { bookPrice = await externalAPIBookPriceService.GetResources(Consts.ExternalAPI.BookPrice(bookId)); })
                .Wait();

            bookDetails.CurrentPrice = bookPrice != null ? bookPrice.Price : -1;

            return bookDetails;
        }

        public IEnumerable<BookStatusDto> GetBookStatuses(Guid bookId)
        {
            var book = dbContext.Books
                .AsNoTracking()
                .Where(b => b.Id == bookId)
                .Include(b => b.StatusHistories)
                .FirstOrDefault();

            return book?.StatusHistories
                .Select(sh => new BookStatusDto
                {
                    Id = sh.Id,
                    BookId = bookId,
                    ModifiedDate = sh.ModifiedDate,
                    Status = sh.Status
                })
                .OrderBy(sh => sh.ModifiedDate);
        }

        public async Task<Guid> InsertBook(InsertBookDto bookDto)
        {
            var newBook = Book.Create(Guid.NewGuid(), null, bookDto.Title, bookDto.Language,
                                bookDto.PublicationDate, bookDto.Genre);

            var author = bookDto.AuthorId != null ? await authors.FindAsync((Guid)bookDto.AuthorId) : null;
            if (author != null)
            {
                newBook.AddAuthor(author);
            }

            var newStatus = StatusHistory.Create(Guid.NewGuid(), newBook, DateTime.Now, Statuses.InStock);
            newBook.AddStatus(newStatus);

            await books.AddAsync(newBook);

            newBook.ChangeStatus(newStatus);
            await books.UpdateAsync(newBook);

            logger.LogInformation($"Book {newBook.Id} created with status {newStatus.Id}.");

            return newBook.Id;
        }

        public void ChangeBookStatus(Guid bookId, Statuses status)
        {
            var book = books.Find(bookId);

            var newStatus = StatusHistory.Create(Guid.NewGuid(), book, DateTime.Now, status);

            statusHistories.Add(newStatus);

            book.ChangeStatus(newStatus);
            books.Update(book);

            logger.LogInformation($"Book {book.Id} status updated for {newStatus.Id}.");
        }
    }
}