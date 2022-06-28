using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace LibraryAPI.Services
{
    public interface IExternalAPIService<TResult>
        where TResult : class
    {
        Task<TResult> GetResources(string url);
    }

    public class ExternalAPIService<TResult> : IExternalAPIService<TResult>
        where TResult : class
    {
        private readonly HttpClient client = new();

        public async Task<TResult> GetResources(string url)
        {
            try
            {
                var response = await client.GetAsync(url);
                var result = JsonConvert.DeserializeObject<TResult>(await response.Content.ReadAsStringAsync());

                return result;
            }
            catch { }

            return default;
        }
    }
}