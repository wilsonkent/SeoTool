using SeoTool.Services.Interface;
using System.Net.Http;
using System.Threading.Tasks;

namespace SeoTool.Services.WebScraping
{
    public class WebScrapingService : IWebScrapingService
    {
        private readonly HttpClient _httpClient;

        public WebScrapingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:44.0) Gecko/20100101 Firefox/44.0");
        }

        public async Task<string> ScrapeUrl(string url)
        {
            var response = _httpClient.GetStringAsync(url);
            return await response;
        }
    }
}
