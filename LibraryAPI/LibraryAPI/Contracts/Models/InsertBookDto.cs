using LibraryAPI.Domain.Books;
using System;

namespace LibraryAPI.Contracts.Models
{
    public class InsertBookDto
    {
        public string Title { get; set; }
        public string Language { get; set; }
        public DateTime? PublicationDate { get; set; }
        public BookGenres? Genre { get; set; }
        public Guid? AuthorId { get; set; }
    }
}