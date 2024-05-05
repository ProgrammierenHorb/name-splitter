using NameSplitter.DTOs;
using NameSplitter.Enum;
using NameSplitter.Events;
using NameSplitter.Services;
using NameSplitter.Views;
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
        private bool _dialogOpen = false;
        private bool _error = false;
        private string _errorMessage = string.Empty;
        private IEventAggregator _eventAggregator;
        private string _firstname = "";
        private GenderEnum _gender;
        private string _input = "";
        private string _salutation = "";
        private string _surname = "";
        private string _titles = "";

        #endregion privateVariables

        #region Buttons

        public DelegateCommand AddTitleCommand { get; set; }
        public DelegateCommand ButtonParse { get; set; }
        public DelegateCommand ButtonReset { get; set; }

        #endregion Buttons

        #region ObservableCollections

        public ObservableCollection<string> AvailableTitles { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<StructuredName> EnteredElements { get; set; } = new ObservableCollection<StructuredName>();

        #endregion ObservableCollections

        #region Properties

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

        public GenderEnum Gender
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

        //private ParsedElements _parsedView = new ParsedElements();

        public SplitterViewModel( IApiClient apiClient, IEventAggregator eventAggregator )
        {
            _apiClient = apiClient;
            _eventAggregator = eventAggregator;

            ButtonParse = new DelegateCommand(ButtonParseHandler);
            ButtonReset = new DelegateCommand(ButtonResetHandler);
            AddTitleCommand = new DelegateCommand(AddTitleCommandHandler);

            _eventAggregator.GetEvent<ParseEvent>().Subscribe(ButtonParseHandler);
            _eventAggregator.GetEvent<UpdateParsedList>().Subscribe(UpdateParsedElementsList);
            _eventAggregator.GetEvent<OpenParsedElementsView>().Subscribe(OpenParsedElementsView);

            Task.Run(async () =>
            {
                var titles = await _apiClient.GetTitles();
                Application.Current.Dispatcher.Invoke(() =>
                {
                    AvailableTitles.AddRange(titles);
                });
            });
        }

        private void AddTitleCommandHandler()
        {
            if( !_dialogOpen )
            {
                _dialogOpen = true;

                AddTitleView _view = new AddTitleView();
                _view.DataContext = new AddTitleViewModel(_view, _apiClient);
                _view.ShowDialog();

                _dialogOpen = false;

                //refresh
                Task.Run(async () =>
                {
                    var titles = await _apiClient.GetTitles();
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        AvailableTitles.AddRange(titles);
                    });
                });
            }
        }

        private void ButtonParseHandler()
        {
            if( !_dialogOpen )
            {
                _dialogOpen = true;
                Task.Run(async () =>
                {
                    var result = await _apiClient.Parse(Input);

                    Error = result.Error;
                    ErrorMessage = result.ErrorMessage;

                    if( result.StructuredName is not null )
                    {
                        if( result.StructuredName.Titles != null )
                            Titles = string.Join(", ", result.StructuredName.Titles);

                        Gender = result.StructuredName.Gender;
                        FirstName = result.StructuredName.FirstName;
                        LastName = result.StructuredName.LastName;
                    }

                    //der dispatcher-thread wird benötigt, um die GUI anpassen zu können
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if( Error && ErrorMessage.Contains("Es konnte keine Verbindung hergestellt werden, da der Zielcomputer die Verbindung verweigerte") )
                        {
                            MessageBox.Show("Der Server konnte nicht erreicht werden \nBitte überprüfen Sie, ob das Backend gestartert wurde. " +
                                "Den Status können Sie unter http://localhost:8080/api/status abfragen.", "Keine Verbindung zum Server möglich", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                        }
                        else
                        {
                            OpenParsedElementsView(result);
                        }
                    });
                    _dialogOpen = false;
                });
            }
        }

        private void ButtonResetHandler()
        {
            EnteredElements.Clear();
        }

        private string ConvertGenderEnumToString( GenderEnum gender )
        {
            return gender switch
            {
                GenderEnum.MALE => "Männlích",
                GenderEnum.FEMALE => "Weiblich",
                GenderEnum.DIVERSE => "Divers",
                _ => "Unbekannt",
            };
        }

        private void OpenParsedElementsView( ParseResponseDto parseResponse )
        {
            ParsedElements _parsedView = new ParsedElements();
            _parsedView.DataContext = new ParsedElementsViewModel(_apiClient, _eventAggregator, _parsedView, parseResponse);
            _parsedView.ShowDialog();
        }

        private void UpdateParsedElementsList( StructuredName updatedList )
        {
            updatedList.GenderString = ConvertGenderEnumToString(updatedList.Gender);
            EnteredElements.Add(updatedList);
        }
    }
}