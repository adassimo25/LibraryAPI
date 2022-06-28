using System;

namespace LibraryAPI.Contracts.Dtos
{
    public class BookPrice
    {
        public Guid Id { get; set; }
        public double Price { get; set; }
    }
}