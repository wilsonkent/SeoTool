using SeoTool.Commands;
using SeoTool.Services.Interface;
using SeoTool.Models;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace SeoTool.ViewModels
{
    public class SeoViewModel : INotifyPropertyChanged, IExecutableModel
    {
        private const string GoogleSearchBaseUrl = "https://www.google.com.au/search?num=100&q=";

        private readonly SeoModel _search;
        private readonly IWebScrapingService _webScrapingService;
        private readonly IHtmlParsingService _htmlParsingService;
        private readonly ISchedulingService _schedulingService;
        private readonly int _scheduleSecondsInterval;

        public SeoViewModel(IWebScrapingService webScrapingService, IHtmlParsingService htmlParsingService, ISchedulingService schedulingService, int scheduleSecondsInterval)
        {
            _search = new SeoModel
            {
                Keywords = "conveyancing software",
                SearchString = "www.smokeball.com.au",
                Output = String.Empty
            };

            _keywordsIsEnabled = true;
            _searchStringIsEnabled = true;

            StartCommand = new RelayCommand(async o => await StartSeo(), StartCommandEnabled);
            StopCommand = new RelayCommand(async o => await StopSeo(), StopCommandEnabled);

            //services
            _webScrapingService = webScrapingService;
            _htmlParsingService = htmlParsingService;
            _schedulingService = schedulingService;

            //defaults
            _scheduleSecondsInterval = scheduleSecondsInterval;
        }


        #region Properties
        public string Keywords
        {
            get { return _search.Keywords; }
            set
            {
                if (_search.Keywords != value)
                {
                    _search.Keywords = value;
                    OnPropertyChange("Keywords");
                }
            }
        }

        public string SearchString
        {
            get { return _search.SearchString; }
            set
            {
                if (_search.SearchString != value)
                {
                    _search.SearchString = value;
                    OnPropertyChange("SearchString");
                }
            }
        }

        public string Output
        {
            get { return _search.Output; }
            set
            {
                if (_search.Output != value)
                {
                    _search.Output = value;
                    OnPropertyChange("Output");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private bool _keywordsIsEnabled;
        public bool KeywordsIsEnabled
        {
            get { return _keywordsIsEnabled; }

            set
            {
                if (_keywordsIsEnabled == value)
                {
                    return;
                }

                _keywordsIsEnabled = value;
                OnPropertyChange("KeywordsIsEnabled");
            }
        }

        private bool _searchStringIsEnabled;
        public bool SearchStringIsEnabled
        {
            get { return _searchStringIsEnabled; }

            set
            {
                if (_searchStringIsEnabled == value)
                {
                    return;
                }

                _searchStringIsEnabled = value;
                OnPropertyChange("SearchStringIsEnabled");
            }
        }

        #endregion

        #region Commands
        //Commands
        private bool _startCommandEnabled = true;
        private bool _stopCommandEnabled = false;
        public ICommand StartCommand { get; set; }
        public ICommand StopCommand { get; set; }

        public async Task StartSeo()
        {
            _startCommandEnabled = KeywordsIsEnabled = SearchStringIsEnabled = false;
            _stopCommandEnabled = true;

            //output
            _search.Output += $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}: Starting scheduler with intervals of {_scheduleSecondsInterval} seconds... { Environment.NewLine }";
            OnPropertyChange("Output");

            //schedule job
            await _schedulingService.StartSchedule(_scheduleSecondsInterval, this);
        }

        private async Task RunScrape()
        {
            try
            {
                var scrapeResult = await _webScrapingService.ScrapeUrl($"{GoogleSearchBaseUrl}{Keywords.Trim().Replace(" ", "+")}");
                var result = _htmlParsingService.FindTextInHtml(scrapeResult, SearchString);

                _search.Output += $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}: {result} { Environment.NewLine }";
                OnPropertyChange("Output");
            }
            catch(Exception e)
            {
                _search.Output += $"{DateTime.Now:yyyy/MM/dd HH:mm:ss}: Exception caught: {e.Message} { Environment.NewLine }";
                OnPropertyChange("Output");

                await StopSeo();
                Application.Current.Dispatcher.Invoke(() => { CommandManager.InvalidateRequerySuggested(); });
            }
        }

        private bool StartCommandEnabled(object state)
        {
            return _startCommandEnabled;
        }

        public async Task StopSeo()
        {
            _startCommandEnabled = KeywordsIsEnabled = SearchStringIsEnabled = true;
            _stopCommandEnabled = false;

            //stop schedule
            await _schedulingService.StopSchedule();

            //output
            _search.Output += $"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")}: Scheduler stopped. { Environment.NewLine }";
            OnPropertyChange("Output");
        }

        private bool StopCommandEnabled(object state)
        {
            return _stopCommandEnabled;
        }

        #endregion

        #region IExecutable
        public async Task ExecuteTask()
        {
            await RunScrape();
        }
        #endregion
    }
}
