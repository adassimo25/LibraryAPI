using FluentValidation;
using LibraryAPI.Contracts.CQRS.Commands;
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
    public class InsertBookCV : CQValidator<InsertBook>
    {
        public InsertBookCV(LibraryDbContext dbContext)
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

    public class InsertBookCH : ICommandHandler<InsertBook>
    {
        private readonly IRepository<Book, Guid> books;
        private readonly IRepository<Author, Guid> authors;
        private readonly ILogger<InsertBookCH> logger;

        public InsertBookCH(IRepository<Book, Guid> books, IRepository<Author, Guid> authors,
            ILogger<InsertBookCH> logger)
        {
            this.books = books;
            this.authors = authors;
            this.logger = logger;
        }

        public async Task ExecuteAsync(InsertBook command, LibraryContext context)
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
        }
    }
}