using LibraryAPI.Contracts.Dtos;
using LibraryAPI.Contracts.Requests;
using LibraryAPI.Domain.StatusHistories;
using LibraryAPI.Services.Interfaces;
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
        private readonly IBooksService booksService;

        public LibraryController(IServiceProvider serviceProvider, IBooksService booksService) : base(serviceProvider)
        {
            this.booksService = booksService;
        }

        /// <summary>
        /// Get list of books (Id and Title)
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="limit">Number of books on each page</param>
        /// <returns>List of Books sorted by book title</returns>
        [HttpGet]
        public ActionResult<IEnumerable<BookDto>> GetBooks(int page = 0, int limit = 10)
        {
            var vResult = ValidateRequest<GetBooksRequest>(new GetBooksRequest());

            return vResult ?? Ok(booksService.GetBooks(page, limit));
        }

        /// <summary>
        /// Get details of a book
        /// </summary>
        /// <param name="bookId">Book Id</param>
        /// <returns>Details of a book</returns>
        [HttpGet("details/{bookId}")]
        public ActionResult<BookDetailsDto> GetBookDetails([FromRoute] Guid bookId)
        {
            var vResult = ValidateRequest<GetBookDetailsRequest>(new GetBookDetailsRequest { BookId = bookId });

            return vResult ?? Ok(booksService.GetBookDetails(bookId));
        }

        /// <summary>
        /// Get history of book statuses sorted by date (oldest to newest)
        /// </summary>
        /// <param name="bookId">Book Id</param>
        /// <returns>List of BookStatuses</returns>
        [HttpGet("statuses/{bookId}")]
        public ActionResult<IEnumerable<BookStatusDto>> GetBookStatuses([FromRoute] Guid bookId)
        {
            var vResult = ValidateRequest<GetBookStatusesRequest>(new GetBookStatusesRequest { BookId = bookId });

            return vResult ?? Ok(booksService.GetBookStatuses(bookId));
        }

        /// <summary>
        /// Insert new book to the library
        /// </summary>
        /// <param name="insertBookDto">Book to create details</param>
        /// <returns>Id of new book</returns>
        [HttpPost]
        public async Task<ActionResult<Guid>> InsertBook([FromBody] InsertBookDto insertBook)
        {
            var vResult = ValidateRequest<InsertBookDto>(insertBook);

            return vResult ?? Ok(await booksService.InsertBook(insertBook));
        }

        /// <summary>
        /// Change status of a book
        /// </summary>
        /// <param name="bookId">Bookd Id</param>
        /// <param name="status">New book status</param>
        [HttpPost("status/{bookId}")]
        public ActionResult ChangeBookStatus([FromRoute] Guid bookId, [FromBody] Statuses status)
        {
            var vResult = ValidateRequest<ChangeBookStatusRequest>(new ChangeBookStatusRequest { BookId = bookId, Status = status });
            if (vResult != null)
            {
                return vResult;
            }

            booksService.ChangeBookStatus(bookId, status);

            return Ok();
        }
    }
}