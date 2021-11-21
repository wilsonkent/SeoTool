using SeoTool.Services.HtmlParsing;
using SeoTool.Services.Scheduling;
using SeoTool.Services.WebScraping;
using SeoTool.ViewModels;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SeoTool.Test.Integration
{
    public class SeoViewModelTest
    {
        private readonly SeoViewModel _viewModel;

        public SeoViewModelTest()
        {
            _viewModel = new SeoViewModel(new WebScrapingService(new System.Net.Http.HttpClient()), 
                new GoogleResultHtmlParsingService(), 
                new SchedulingService(), 
                5);
        }

        [Fact]
        public async Task SeoScrap_ValidUrl_OutputsMessage()
        {
            await _viewModel.StartSeo();
            await Task.Delay(10000);
            await _viewModel.StopSeo();

            Assert.Contains("Starting scheduler", _viewModel.Output);
            Assert.True(_viewModel.Output.Split(Environment.NewLine).Length > 3);
            Assert.Contains("Scheduler stopped", _viewModel.Output);
        }
    }
}
