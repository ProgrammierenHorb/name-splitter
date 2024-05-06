using NameSplitter.DTOs;
using NameSplitter.Services;
using NameSplitter.Views;
using Prism.Commands;
using Prism.Mvvm;
using System.Threading.Tasks;
using System.Windows;

namespace NameSplitter.ViewModels
{
    /// <summary>
    /// Viewmodel for AddTitle view,
    /// used to add new titles
    /// </summary>
    public class AddTitleViewModel : BindableBase
    {
        private IApiClient _apiClient;
        private bool _useRegex = false;
        private AddTitleView _window;

        /// <summary>
        /// constructor and initailsation of delegate commands
        /// </summary>
        /// <param name="window"></param>
        /// <param name="apiClient"></param>
        public AddTitleViewModel( AddTitleView window, IApiClient apiClient )
        {
            _window = window;
            _apiClient = apiClient;
            AddTitleCommand = new DelegateCommand(AddTitleCommandHandlerAsync);

            ButtonCancle = new DelegateCommand(CancleButtonHandler);
        }

        public DelegateCommand AddTitleCommand { get; set; }
        public DelegateCommand ButtonCancle { get; set; }

        /// <summary>
        /// Binding property for new title
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// Binding property for new regex of the title
        /// </summary>
        public string InputRegex { get; set; }

        /// <summary>
        /// Binding property for the checkbox
        /// </summary>
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

        /// <summary>
        /// Binding property for the visibility of the regex textbox
        /// </summary>
        public Visibility Visibility
        {
            get
            {
                if (UseRegex) return Visibility.Visible;
                return Visibility.Hidden;
            }
        }

        /// <summary>
        /// Command to save the new title in backend using the api
        /// </summary>
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

        /// <summary>
        /// Cancel command to close the window
        /// </summary>
        public void CancleButtonHandler()
        { _window.Close(); }
    }
}