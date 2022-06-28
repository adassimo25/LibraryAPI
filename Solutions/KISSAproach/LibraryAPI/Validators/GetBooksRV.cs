using LibraryAPI.Contracts.Requests;
using LibraryAPI.DataAccess;
using LibraryAPI.Validators.Common;

namespace LibraryAPI.Validators
{
    public class GetBooksRV : RequestValidator<GetBooksRequest>
    {
        public GetBooksRV(LibraryDbContext dbContext)
        {
        }
    }
}