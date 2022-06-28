using FluentValidation;
using LibraryAPI.Contracts.Requests;
using LibraryAPI.DataAccess;
using LibraryAPI.Validators.Common;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Validators
{
    public class GetBookDetailsRV : RequestValidator<GetBookDetailsRequest>
    {
        public GetBookDetailsRV(LibraryDbContext dbContext)
        {
            RuleFor(cmd => cmd)
                .MustAsync(async (cmd, ct) =>
                {
                    var bookExists = await dbContext.Books
                        .AnyAsync(b => b.Id == cmd.BookId, ct);
                    return bookExists;
                }).WithErrorCode(GetBookDetailsRequest.ErrorCodes.BookDoesNotExist);
        }
    }
}