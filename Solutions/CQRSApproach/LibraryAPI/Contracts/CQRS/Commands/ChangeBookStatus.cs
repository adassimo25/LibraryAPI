using LibraryAPI.Domain.StatusHistories;
using System;

namespace LibraryAPI.Contracts.CQRS.Commands
{
    public class ChangeBookStatus
    {
        public Guid BookId { get; set; }
        public Statuses Status { get; set; }

        public static class ErrorCodes
        {
            public const string BookDoesNotExist = "Book does not exist";
        }
    }
}