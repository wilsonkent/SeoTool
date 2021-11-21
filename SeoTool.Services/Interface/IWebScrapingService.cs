using System.Threading.Tasks;

namespace SeoTool.Services.Interface
{
    public interface IWebScrapingService
    {
        Task<string> ScrapeUrl(string url);
    }
}