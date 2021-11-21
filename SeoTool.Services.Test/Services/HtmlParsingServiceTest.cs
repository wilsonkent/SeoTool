using SeoTool.Services.HtmlParsing;
using SeoTool.Services.Interface;
using System.IO;
using Xunit;

namespace SeoTool.Services.Test.Services
{
    public class HtmlParsingServiceTest
    {
        private const string HtmlTestFileName = "./Resources/SampleGoogleResult.html";

        private readonly IHtmlParsingService _htmlParsingService;
        private readonly string _htmlTestString;

        public HtmlParsingServiceTest()
        {
            _htmlParsingService = new GoogleResultHtmlParsingService();
            _htmlTestString = File.ReadAllText(HtmlTestFileName);
        }

        [Fact]
        public void FindTextInHtml_SampleWithOneResult_ReturnCorrectString()
        {
            var textToSearch = "www.smokeball.com.au";
            var result = _htmlParsingService.FindTextInHtml(_htmlTestString, textToSearch);

            Assert.Equal("2", result);
        }

        [Fact]
        public void FindTextInHtml_SampleWithMultipleResults_ReturnCorrectString()
        {
            var textToSearch = "smokeball";
            var result = _htmlParsingService.FindTextInHtml(_htmlTestString, textToSearch);

            Assert.Equal("2,54,62,71,99,101", result);
        }

        [Fact]
        public void FindTextInHtml_SampleNoResults_ReturnZeroString()
        {
            var textToSearch = "LEAP Conveyancer";
            var result = _htmlParsingService.FindTextInHtml(_htmlTestString, textToSearch);

            Assert.Equal("0", result);
        }
    }
}
