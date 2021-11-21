using HtmlAgilityPack;
using SeoTool.Services.Interface;
using SeoTool.Services.WebScraping;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace SeoTool.Services.Test.Services
{
    public class WebScrapingServiceTest
    {
        private readonly IWebScrapingService _webScrapingService;

        public WebScrapingServiceTest()
        {
            var httpClient = new HttpClient();
            _webScrapingService = new WebScrapingService(httpClient);
        }

        [Theory]
        [InlineData("https://www.google.com.au/search?num=100&q=conveyancing+software")]
        [InlineData("https://en.wikipedia.org/wiki/List_of_days_of_the_year")]
        [InlineData("https://stackoverflow.com/questions")]
        public async Task ScrapeUrl_InputUrl_ReturnHtmlContent(string url)
        {
            var result = await _webScrapingService.ScrapeUrl(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(result);
            //if all elements are text - then is not html
            var isHtml = !doc.DocumentNode.Descendants().All(n => n.NodeType == HtmlNodeType.Text);

            Assert.True(isHtml);
        }

        [Fact]
        public async Task ScrapeUrl_InvalidUrl_ThrowsException()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await _webScrapingService.ScrapeUrl("hello world"));
        }
    }
}
