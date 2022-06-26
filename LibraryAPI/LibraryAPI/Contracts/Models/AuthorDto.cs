using System;

namespace LibraryAPI.Contracts.Models
{
    public class AuthorDto
    {
        /// <summary>
        /// First name and last name separated with space
        /// </summary>
        public string Name { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}