using System;

namespace LibraryAPI.DataAccess
{
    public static class Consts
    {
        public static class ExternalAPI
        {
            public static string BookPrice(Guid id) => $"http://60c35511917002001739e94a.mockapi.io/api/v1/Prices/{id}";
        }

        public static class StringLengths
        {
            public const int TinyString = 50;
            //public const int ShortString = 256;
            //public const int MediumString = 512;
            //public const int LongString = 1024;
            //public const int MaxString = 2056;
        }
    }
}