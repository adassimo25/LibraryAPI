using FluentValidation;
using LibraryAPI.Contracts.CQRS.Queries;
using LibraryAPI.DataAccess;
using LibraryAPI.DataAccess.Repositories;
using LibraryAPI.Domain.Authors;
using LibraryAPI.Domain.Books;
using LibraryAPI.Domain.StatusHistories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using SL = LibraryAPI.DataAccess.Consts.StringLengths;

namespace LibraryAPI.CQRS.Commands
{
    public class InsertBookQV : CQValidator<InsertBook>
    {
        public InsertBookQV(LibraryDbContext dbContext)
        {
            RuleFor(cmd => cmd)
                .MustAsync(async (cmd, ct) =>
                {
                    var authorExists = cmd.AuthorId == null || await dbContext.Authors.AnyAsync(b => b.Id == cmd.AuthorId, ct);
                    return authorExists;
                }).WithErrorCode(InsertBook.ErrorCodes.AuthorDoesNotExist);

            RuleFor(cmd => cmd.Language)
               .MaximumLength(SL.TinyString)
               .When(cmd => cmd.Language != null)
                   .WithErrorCode(InsertBook.ErrorCodes.LanguageTooLong);
        }
    }

    public class InsertBookQH : IQueryHandler<InsertBook, Guid>
    {
        private readonly IRepository<Book, Guid> books;
        private readonly IRepository<Author, Guid> authors;
        private readonly ILogger<InsertBookQH> logger;
        private readonly LibraryDbContext dbContext;

        public InsertBookQH(IRepository<Book, Guid> books, IRepository<Author, Guid> authors,
            ILogger<InsertBookQH> logger, LibraryDbContext dbContext)
        {
            this.books = books;
            this.authors = authors;
            this.logger = logger;
            this.dbContext = dbContext;
        }

        public async Task<Guid> ExecuteAsync(InsertBook command, LibraryContext context)
        {
            var newBook = Book.Create(Guid.NewGuid(), null, command.Title, command.Language,
                                command.PublicationDate, command.Genre);

            var author = command.AuthorId != null ? await authors.FindAsync((Guid)command.AuthorId) : null;
            if (author != null)
            {
                newBook.AddAuthor(author);
            }

            var newStatus = StatusHistory.Create(Guid.NewGuid(), newBook, DateTime.Now, Statuses.InStock);
            newBook.AddStatus(newStatus);

            books.Add(newBook);

            newBook.ChangeStatus(newStatus);
            books.Update(newBook);

            logger.LogInformation($"Book {newBook.Id} created with status {newStatus.Id}.");

            return newBook.Id;
        }
    }
}