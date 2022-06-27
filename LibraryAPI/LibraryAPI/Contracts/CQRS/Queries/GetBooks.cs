using LibraryAPI.Contracts.Common;
using LibraryAPI.Contracts.Dtos;

namespace LibraryAPI.Contracts.CQRS.Queries
{
    public class GetBooks : PaginatedQuery<BookDto>
    {
        public static class ErrorCodes
        {
        }
    }
}