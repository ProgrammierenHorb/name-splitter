using NameSplitter.DTOs;
using NameSplitter.Services;
using NameSplitter.Views;
using Prism.Commands;
using Prism.Mvvm;
using System.Threading.Tasks;
using System.Windows;

namespace NameSplitter.ViewModels
{
    public class AddTitleViewModel : BindableBase
    {
        private IApiClient _apiClient;
        private bool _useRegex = false;
        private AddTitleView _window;

        public AddTitleViewModel( AddTitleView window, IApiClient apiClient )
        {
            _window = window;
            _apiClient = apiClient;
            AddTitleCommand = new DelegateCommand(AddTitleCommandHandlerAsync);

            ButtonCancle = new DelegateCommand(CancleButtonHandler);
        }

        public DelegateCommand AddTitleCommand { get; set; }
        public DelegateCommand ButtonCancle { get; set; }
        public string Input { get; set; }
        public string InputRegex { get; set; }

        public bool UseRegex
        {
            get
            {
                return _useRegex;
            }
            set
            {
                _useRegex = value;
                RaisePropertyChanged(nameof(Visibility));
            }
        }

        public Visibility Visibility
        {
            get
            {
                if (UseRegex) return Visibility.Visible;
                return Visibility.Hidden;
            }
        }

        public async void AddTitleCommandHandlerAsync()
        {
            bool response;
            if (UseRegex)
                response = await _apiClient.AddTitle(Input, InputRegex);
            else
                response = await _apiClient.AddTitle(Input, Input);
            if (response)
            {
                _window.Close();
            }
            else
            {
                MessageBox.Show("Der Server konnte nicht erreicht werden \nBitte überprüfen Sie, ob das Backend gestartert wurde. " +
                                "Den Status können Sie unter http://localhost:8080/api/status abfragen.", "Keine Verbindung zum Server möglich", MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        public void CancleButtonHandler()
        { _window.Close(); }
    }
}