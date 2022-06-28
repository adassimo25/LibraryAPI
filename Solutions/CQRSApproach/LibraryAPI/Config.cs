using Microsoft.Extensions.Configuration;

namespace LibraryAPI
{
    public static class Config
    {
        public static class App
        {
            public static string ApiDomain(IConfiguration cfg) => cfg.GetConnectionString("ApiDomain");
        }

        public static class Services
        {
            public static class Database
            {
                public static string ConnectionString(IConfiguration cfg) => cfg.GetConnectionString("Database");
            }
        }
    }
}