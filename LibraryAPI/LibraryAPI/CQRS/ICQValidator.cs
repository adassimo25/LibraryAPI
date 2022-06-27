using FluentValidation.Results;
using LibraryAPI.DataAccess;
using System.Threading.Tasks;

namespace LibraryAPI.CQRS
{
    public interface ICQValidator<TContract>
        where TContract : class
    {
        Task<ValidationResult> Validate(TContract instance, LibraryContext context);
    }
}