using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
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
        private readonly ILogger<ExternalAPIService<TResult>> logger;

        public ExternalAPIService(ILogger<ExternalAPIService<TResult>> logger)
        {
            this.logger = logger;
        }

        public async Task<TResult> GetResources(string url)
        {
            HttpResponseMessage response = new();
            string responseContent = new("<no-content>");

            try
            {
                response = await client.GetAsync(url);
                responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<TResult>(responseContent);

                return result;
            }
            catch (Exception e)
            {
                logger.LogError($"ExternalAPI response status: {response.StatusCode}");
                logger.LogError($"ExternalAPI response content: {responseContent}");
                logger.LogError(e.Message);
            }

            return default;
        }
    }
}