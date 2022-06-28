using LibraryAPI.Domain.Authors;
using LibraryAPI.Domain.StatusHistories;
using System;
using System.Collections.Generic;

namespace LibraryAPI.Domain.Books
{
    public class Book
    {
        private readonly List<Author> authors = new();
        private readonly List<StatusHistory> statusHistories = new();

        public Guid Id { get; private init; }

        // Before seeding data
        //public Guid? CurrentStatusId { get; private set; } = null;
        // After seeding data
        public StatusHistory CurrentStatus { get; private set; } = null;

        public string Title { get; private set; } = null;
        public string Language { get; private set; } = null;
        public DateTime? PublicationDate { get; private init; } = null;
        public BookGenres? Genre { get; private init; } = null;
        public int? PageNumber { get; private init; } = null;
        public IReadOnlyList<Author> Authors => authors;
        public IReadOnlyList<StatusHistory> StatusHistories => statusHistories;

        public static Book Create(Guid id, StatusHistory currentStatus, string title, string language,
            DateTime? publicationDate, BookGenres? genre, int? pageNumber = null)
        {
            return new()
            {
                Id = id,
                CurrentStatus = currentStatus,
                Title = title,
                Language = language,
                PublicationDate = publicationDate,
                Genre = genre,
                PageNumber = pageNumber
            };
        }

        public void AddAuthor(Author author)
        {
            authors.Add(author);
        }

        public void AddStatus(StatusHistory status)
        {
            statusHistories.Add(status);
        }

        public void ChangeStatus(StatusHistory status)
        {
            CurrentStatus = status;
        }
    }

    public enum BookGenres
    {
        Food,
        History,
        Memoir,
        Politics,
        CrimeThriller,
        ScienceFiction
    }
}