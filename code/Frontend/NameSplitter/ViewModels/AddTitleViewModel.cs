using NameSplitter.Services;
using NameSplitter.Views;
using Prism.Commands;
using System.Threading.Tasks;
using System.Windows;

namespace NameSplitter.ViewModels
{
    public class AddTitleViewModel
    {
        private IApiClient _apiClient;
        private AddTitleView _window;

        public AddTitleViewModel( AddTitleView window, IApiClient apiClient )
        {
            _window = window;
            _apiClient = apiClient;
            AddTitleCommand = new DelegateCommand(AddTitleCommandHandler);

            ButtonCancle = new DelegateCommand(CancleButtonHandler);
        }

        public DelegateCommand AddTitleCommand { get; set; }
        public DelegateCommand ButtonCancle { get; set; }
        public string Input { get; set; }
        public bool UseRegex { get; set; }

        public void AddTitleCommandHandler()
        {
            var response = _apiClient.AddTitle(Input, UseRegex);
            if (response.Result.ErrorMessage == null) { _window.Close(); }
        }

        public void CancleButtonHandler()
        { _window.Close(); }
    }
}