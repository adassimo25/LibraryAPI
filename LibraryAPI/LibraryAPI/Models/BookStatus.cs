using LibraryAPI.Enums;
using System;

namespace LibraryAPI.Models
{
    public class BookStatus
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Statuses Status { get; set; }
    }
}
