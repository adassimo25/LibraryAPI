using LibraryAPI.Domain.Books;
using System;
using System.Collections.Generic;

namespace LibraryAPI.Domain.Authors
{
    public class Author
    {
        private readonly List<Book> books = new();

        public Guid Id { get; private init; }
        public string FirstName { get; private set; } = null;
        public string LastName { get; private set; } = null;
        public DateTime? DateOfBirth { get; private set; } = null;
        public IReadOnlyList<Book> Books => books;

        public static Author Create(Guid id, string firstName, string lastName, DateTime dateOfBirth)
        {
            return new()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
            };
        }
    }
}