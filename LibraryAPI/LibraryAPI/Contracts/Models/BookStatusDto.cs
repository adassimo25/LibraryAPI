using LibraryAPI.Domain.StatusHistories;
using System;

namespace LibraryAPI.Contracts.Models
{
    public class BookStatusDto
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Statuses Status { get; set; }
    }
}