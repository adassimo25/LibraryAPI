using LibraryAPI.Contracts.CQRS.Commands;
using LibraryAPI.Contracts.CQRS.Queries;
using LibraryAPI.Contracts.Dtos;
using LibraryAPI.Domain.StatusHistories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibraryController : BaseLibraryAPIController
    {
        public LibraryController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        /// <summary>
        /// Get list of books (Id and Title)
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="limit">Number of books on each page</param>
        /// <returns>List of Books sorted by book title</returns>
        [HttpGet]
        public Task<IActionResult> GetBooks(int page = 0, int limit = 10)
        {
            var query = new GetBooks { PageNumber = page, PageSize = limit };
            return HandleQueryAsync<GetBooks, IEnumerable<BookDto>>(query);
        }

        /// <summary>
        /// Get details of a book
        /// </summary>
        /// <param name="bookId">Book Id</param>
        /// <returns>Details of a book</returns>
        [HttpGet("details/{bookId}")]
        public Task<IActionResult> GetBookDetails([FromRoute] Guid bookId)
        {
            var query = new GetBookDetails { BookId = bookId };
            return HandleQueryAsync<GetBookDetails, BookDetailsDto>(query);
        }

        /// <summary>
        /// Get history of book statuses sorted by date (oldest to newest)
        /// </summary>
        /// <param name="bookId">Book Id</param>
        /// <returns>List of BookStatuses</returns>
        [HttpGet("statuses/{bookId}")]
        public Task<IActionResult> GetBookStatuses([FromRoute] Guid bookId)
        {
            var query = new GetBookStatuses { BookId = bookId };
            return HandleQueryAsync<GetBookStatuses, IEnumerable<BookStatusDto>>(query);
        }

        /// <summary>
        /// Insert new book to the library
        /// </summary>
        /// <param name="insertBookDto">Book to create details</param>
        /// <returns>Id of new book</returns>
        [HttpPost]
        public Task<IActionResult> InsertBook([FromBody] InsertBook insertBook)
        {
            return HandleQueryAsync<InsertBook, Guid>(insertBook);
        }

        /// <summary>
        /// Change status of a book
        /// </summary>
        /// <param name="bookId">Bookd Id</param>
        /// <param name="status">New book status</param>
        [HttpPost("status/{bookId}")]
        public Task<IActionResult> ChangeBookStatus([FromRoute] Guid bookId, [FromBody] Statuses status)
        {
            var command = new ChangeBookStatus { BookId = bookId, Status = status };
            return HandleCommandAsync<ChangeBookStatus>(command);
        }
    }
}