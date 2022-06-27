using LibraryAPI.DataAccess;
using System.Threading.Tasks;

namespace LibraryAPI.CQRS
{
    public interface IQueryHandler<TContract, TResult>
        where TContract : class
    {
        Task<TResult> ExecuteAsync(TContract query, LibraryContext context);
    }
}