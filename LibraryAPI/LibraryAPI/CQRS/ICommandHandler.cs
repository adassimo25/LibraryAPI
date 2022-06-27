using LibraryAPI.DataAccess;
using System.Threading.Tasks;

namespace LibraryAPI.CQRS
{
    public interface ICommandHandler<TContract>
        where TContract : class
    {
        Task ExecuteAsync(TContract command, LibraryContext context);
    }
}