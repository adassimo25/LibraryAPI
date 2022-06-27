using System;

namespace LibraryAPI.Contracts.CQRS.Queries
{
    public class GetBookDetails
    {
        public Guid BookId { get; set; }

        public static class ErrorCodes
        {
            public const string BookDoesNotExist = "Book does not exist";
        }
    }
}