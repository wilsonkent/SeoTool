using Microsoft.Extensions.DependencyInjection;
using SeoTool.Services.HtmlParsing;
using SeoTool.Services.Interface;
using SeoTool.Services.Scheduling;
using SeoTool.Services.WebScraping;
using SeoTool.ViewModels;
using System.Net.Http;
using System.Windows;

namespace SeoTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly SeoViewModel _viewModel;

        public MainWindow()
        {
            //di
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            InitializeComponent();

            //config
            int scheduleSecondsInterval = 86400;

            //bind viewmodel
            var webScrapingService = _serviceProvider.GetService<IWebScrapingService>();
            var htmlParsingService = _serviceProvider.GetService<IHtmlParsingService>();
            var schedulingService = _serviceProvider.GetService<ISchedulingService>();
            _viewModel = new SeoViewModel(webScrapingService, htmlParsingService, schedulingService, scheduleSecondsInterval);
            DataContext = _viewModel;
        }

        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<HttpClient>(new HttpClient());
            services.AddSingleton<IWebScrapingService, WebScrapingService>();
            services.AddSingleton<IHtmlParsingService, GoogleResultHtmlParsingService>();
            services.AddSingleton<ISchedulingService, SchedulingService>();
        }
    }
}
