using LibraryAPI.Contracts.Dtos;
using LibraryAPI.Domain.StatusHistories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryAPI.Services.Interfaces
{
    public interface IBooksService : IService
    {
        IEnumerable<BookDto> GetBooks(int page, int limit);

        BookDetailsDto GetBookDetails(Guid bookId);

        IEnumerable<BookStatusDto> GetBookStatuses(Guid bookId);

        Task<Guid> InsertBook(InsertBookDto bookDto);

        void ChangeBookStatus(Guid bookId, Statuses status);
    }
}