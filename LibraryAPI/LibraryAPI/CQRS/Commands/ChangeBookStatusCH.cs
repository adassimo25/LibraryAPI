using FluentValidation;
using LibraryAPI.Contracts.CQRS.Commands;
using LibraryAPI.DataAccess;
using LibraryAPI.DataAccess.Repositories;
using LibraryAPI.Domain.Books;
using LibraryAPI.Domain.StatusHistories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LibraryAPI.CQRS.Commands
{
    public class ChangeBookStatusCV : CQValidator<ChangeBookStatus>
    {
        public ChangeBookStatusCV(LibraryDbContext dbContext)
        {
            RuleFor(cmd => cmd)
                .MustAsync(async (cmd, ct) =>
                {
                    var bookExists = await dbContext.Books
                        .AnyAsync(b => b.Id == cmd.BookId, ct);
                    return bookExists;
                }).WithErrorCode(ChangeBookStatus.ErrorCodes.BookDoesNotExist);
        }
    }

    public class ChangeBookStatusCH : ICommandHandler<ChangeBookStatus>
    {
        private readonly IRepository<Book, Guid> books;
        private readonly IRepository<StatusHistory, Guid> statusHistories;
        private readonly ILogger<ChangeBookStatusCH> logger;
        private readonly LibraryDbContext dbContext;

        public ChangeBookStatusCH(IRepository<Book, Guid> books, IRepository<StatusHistory, Guid> statusHistories,
            ILogger<ChangeBookStatusCH> logger, LibraryDbContext dbContext)
        {
            this.books = books;
            this.statusHistories = statusHistories;
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public async Task ExecuteAsync(ChangeBookStatus command, LibraryContext context)
        {
            var book = await books.FindAsync(command.BookId);

            var newStatus = StatusHistory.Create(Guid.NewGuid(), book, DateTime.Now, command.Status);

            statusHistories.Add(newStatus);

            book.ChangeStatus(newStatus);
            books.Update(book);

            logger.LogInformation($"Book {book.Id} status updated for {newStatus.Id}.");
        }
    }
}