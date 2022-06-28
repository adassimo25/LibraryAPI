using LibraryAPI.Domain.Books;
using System;

namespace LibraryAPI.Domain.StatusHistories
{
    public class StatusHistory
    {
        public Guid Id { get; private init; }
        public Book Book { get; private init; } = null!;
        public DateTime ModifiedDate { get; private init; }
        public Statuses Status { get; private init; }

        public static StatusHistory Create(Guid id, Book book, DateTime modifiedDate, Statuses status)
        {
            return new()
            {
                Id = id,
                Book = book,
                ModifiedDate = modifiedDate,
                Status = status
            };
        }
    }

    public enum Statuses
    {
        Borrowed,
        InStock,
        Missing,
        Sold
    }
}