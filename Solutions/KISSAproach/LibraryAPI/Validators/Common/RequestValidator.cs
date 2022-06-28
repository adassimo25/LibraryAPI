using FluentValidation;
using FluentValidation.Results;
using LibraryAPI.DataAccess;
using System.Threading.Tasks;

namespace LibraryAPI.Validators.Common
{
    public abstract class RequestValidator<TContract>
        : AbstractValidator<TContract>, IRequestValidator<TContract>
        where TContract : class
    {
        protected readonly LibraryDbContext? dbContext;
        protected LibraryContext context = new(default);

        public RequestValidator(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public RequestValidator()
        { }

        async Task<ValidationResult> IRequestValidator<TContract>.Validate(TContract instance, LibraryContext context)
        {
            this.context = context;

            return await ValidateAsync(instance);
        }
    }
}