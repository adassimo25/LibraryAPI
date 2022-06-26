using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        public static void Register(IServiceCollection services, IConfiguration cfg)
        { }
    }
}