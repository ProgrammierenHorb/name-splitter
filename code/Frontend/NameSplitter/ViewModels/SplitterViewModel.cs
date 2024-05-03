using NameSplitter.DTOs;
using NameSplitter.Events;
using NameSplitter.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace NameSplitter.ViewModels
{
    public class SplitterViewModel: BindableBase
    {
        #region privateVariables

        private IApiClient _apiClient;
        private bool _error = false;
        private string _errorMessage = string.Empty;
        private IEventAggregator _eventAggregator;
        private string _firstname = "";
        private string _gender = "";
        private string _input = "";
        private string _salutation = "";
        private string _standardizedSalutation = "";
        private string _surname = "";
        private string _titles = "";

        #endregion privateVariables

        #region Properties

        #region Buttons

        public DelegateCommand ButtonParse { get; set; }
        public DelegateCommand ButtonReset { get; set; }
        public DelegateCommand ButtonSave { get; set; }

        #endregion Buttons

        #region ObservableCollections

        public ObservableCollection<string> AvailableTitles { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<ParseResponse> EnteredElements { get; set; } = new ObservableCollection<ParseResponse>();

        #endregion ObservableCollections

        public bool Error
        {
            get { return _error; }
            set
            {
                _error = value;
                RaisePropertyChanged(nameof(Error));
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                RaisePropertyChanged(nameof(_errorMessage));
            }
        }

        public string FirstName
        {
            get { return _firstname; }
            set
            {
                _firstname = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }

        public string Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                RaisePropertyChanged(nameof(Gender));
            }
        }

        public string Input
        {
            get { return _input; }
            set
            {
                _input = value;
                RaisePropertyChanged(nameof(Input));
            }
        }

        public string LastName
        {
            get { return _surname; }
            set
            {
                _surname = value;
                RaisePropertyChanged(nameof(LastName));
            }
        }

        public string Salutation
        {
            get { return _salutation; }
            set
            {
                _salutation = value;
                RaisePropertyChanged(nameof(Salutation));
            }
        }

        public string StandardizedSalutation
        {
            get { return _standardizedSalutation; }
            set
            {
                _standardizedSalutation = value;
                RaisePropertyChanged(nameof(StandardizedSalutation));
            }
        }

        public string Titles
        {
            get { return _titles; }
            set
            {
                _titles = value;
                RaisePropertyChanged(nameof(Titles));
            }
        }

        #endregion Properties

        public SplitterViewModel( IApiClient apiClient, IEventAggregator eventAggregator )
        {
            _apiClient = apiClient;
            _eventAggregator = eventAggregator;

            ButtonParse = new DelegateCommand(ButtonParseHandler);
            ButtonReset = new DelegateCommand(ButtonResetHandler);
            ButtonSave = new DelegateCommand(ButtonSaveHandler);

            _eventAggregator.GetEvent<ParseEvent>().Subscribe(ButtonParseHandler);
        }

        private void ButtonParseHandler()
        {
            Task.Run(async () =>
            {
                var result = await _apiClient.Parse(Input);
                if( result.StructuredName != null && result.StructuredName.Titles != null )
                    Titles = string.Join(", ", result.StructuredName.Titles);

                StandardizedSalutation = result.StructuredName?.StandardizedSalutation;
                Gender = result.StructuredName?.Gender;
                FirstName = result.StructuredName?.FirstName;
                LastName = result.StructuredName?.LastName;

                //Der Dispatcher-Thread wird benötigt, um die Collection in der GUI anpassen zu können
                Application.Current.Dispatcher.Invoke(() =>
                {
                    EnteredElements.Add(result);
                });
            });
        }

        private void ButtonResetHandler()
        {
            EnteredElements.Clear();
        }

        private void ButtonSaveHandler()
        {
            //EnteredElements.Clear();
        }
    }
}