using System.Collections.Generic;

namespace LibraryAPI.Contracts.Common
{
    public abstract class PaginatedQuery<TResult>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

    public class PaginatedResult<T>
    {
        public List<T> Elements { get; set; } = null!;
        public int TotalElements { get; set; }
    }
}