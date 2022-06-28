namespace LibraryAPI.Contracts.Requests
{
    public class InsertBookRequest
    {
        public static class ErrorCodes
        {
            public const string AuthorDoesNotExist = "Author does not exist";
            public const string LanguageTooLong = "Language is too long";
        }
    }
}