using FluentValidation.Results;
using LibraryAPI.DataAccess;
using System.Threading.Tasks;

namespace LibraryAPI.Validators.Common
{
    public interface IRequestValidator<TContract>
        where TContract : class
    {
        Task<ValidationResult> Validate(TContract instance, LibraryContext context);
    }
}