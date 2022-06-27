using FluentValidation;
using FluentValidation.Results;
using LibraryAPI.DataAccess;
using System.Threading.Tasks;

namespace LibraryAPI.CQRS
{
    public abstract class CQValidator<TContract>
        : AbstractValidator<TContract>, ICQValidator<TContract>
        where TContract : class
    {
        protected readonly LibraryDbContext? dbContext;
        protected LibraryContext context = new(default);

        public CQValidator(LibraryDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public CQValidator()
        { }

        async Task<ValidationResult> ICQValidator<TContract>.Validate(TContract instance, LibraryContext context)
        {
            this.context = context;

            return await ValidateAsync(instance);
        }
    }
}