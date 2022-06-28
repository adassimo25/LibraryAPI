using LibraryAPI.Domain.Books;
using System;

namespace LibraryAPI.Contracts.CQRS.Queries
{
    public class InsertBook
    {
        public string Title { get; set; }
        public string Language { get; set; }
        public DateTime? PublicationDate { get; set; }
        public BookGenres? Genre { get; set; }
        public Guid? AuthorId { get; set; }

        public static class ErrorCodes
        {
            public const string AuthorDoesNotExist = "Author does not exist";
            public const string LanguageTooLong = "Language is too long";
        }
    }
}