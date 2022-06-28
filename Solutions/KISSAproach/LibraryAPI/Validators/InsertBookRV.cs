using FluentValidation;
using LibraryAPI.Contracts.Dtos;
using LibraryAPI.Contracts.Requests;
using LibraryAPI.DataAccess;
using LibraryAPI.Validators.Common;
using Microsoft.EntityFrameworkCore;
using SL = LibraryAPI.DataAccess.Consts.StringLengths;

namespace LibraryAPI.CQRS.Commands
{
    public class InsertBookRV : RequestValidator<InsertBookDto>
    {
        public InsertBookRV(LibraryDbContext dbContext)
        {
            RuleFor(cmd => cmd)
                .MustAsync(async (cmd, ct) =>
                {
                    var authorExists = cmd.AuthorId == null || await dbContext.Authors.AnyAsync(b => b.Id == cmd.AuthorId, ct);
                    return authorExists;
                }).WithErrorCode(InsertBookRequest.ErrorCodes.AuthorDoesNotExist);

            RuleFor(cmd => cmd.Language)
               .MaximumLength(SL.TinyString)
               .When(cmd => cmd.Language != null)
                   .WithErrorCode(InsertBookRequest.ErrorCodes.LanguageTooLong);
        }
    }
}