using System;

namespace LibraryAPI.Contracts.Requests
{
    public class GetBookStatusesRequest
    {
        public Guid BookId { get; set; }

        public static class ErrorCodes
        {
            public const string BookDoesNotExist = "Book does not exist";
        }
    }
}