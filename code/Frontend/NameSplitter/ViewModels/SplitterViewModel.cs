using NameSplitter.DTOs;
using NameSplitter.Enum;
using NameSplitter.Events;
using NameSplitter.Services;
using NameSplitter.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NameSplitter.ViewModels
{
    /// <summary>
    /// View model responsible for managing the interaction logic and data binding for the Name Splitter application.
    /// It handles parsing user input, displaying parsed elements, managing available titles, and updating the UI accordingly.
    /// </summary>

    public class SplitterViewModel : BindableBase
    {
        #region privateVariables

        private IApiClient _apiClient;
        private bool _dialogOpen = false;
        private string _errorMessage = string.Empty;
        private IEventAggregator _eventAggregator;
        private string _firstname = "";
        private GenderEnum _gender;
        private string _input = "";
        private Guid _key;
        private string _salutation = "";
        private string _surname = "";
        private string _titles = "";

        #endregion privateVariables

        #region Buttons

        public DelegateCommand AddTitleCommand { get; set; }
        public DelegateCommand ButtonAddManually { get; set; }
        public DelegateCommand ButtonParse { get; set; }
        public DelegateCommand ButtonReset { get; set; }

        #endregion Buttons

        #region ObservableCollections

        public ObservableCollection<string> AvailableTitles { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<StructuredName> EnteredElements { get; set; } = new ObservableCollection<StructuredName>();

        #endregion ObservableCollections

        #region Properties

        /// <summary>
        /// Binding property for the error message
        /// </summary>
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                RaisePropertyChanged(nameof(_errorMessage));
            }
        }

        /// <summary>
        /// Binding property for the firstname
        /// </summary>
        public string FirstName
        {
            get { return _firstname; }
            set
            {
                _firstname = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }

        /// <summary>
        /// Binding property for the gender
        /// </summary>
        public GenderEnum Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                RaisePropertyChanged(nameof(Gender));
            }
        }

        /// <summary>
        /// Binding property for the input
        /// </summary>
        public string Input
        {
            get { return _input; }
            set
            {
                _input = value;
                RaisePropertyChanged(nameof(Input));
            }
        }

        /// <summary>
        /// Unique key for each entrie
        /// </summary>
        public Guid Key
        {
            get { return _key; }
        }

        /// <summary>
        /// Binding property for the lastname
        /// </summary>
        public string LastName
        {
            get { return _surname; }
            set
            {
                _surname = value;
                RaisePropertyChanged(nameof(LastName));
            }
        }

        /// <summary>
        /// Binding property for the salutation
        /// </summary>
        public string Salutation
        {
            get { return _salutation; }
            set
            {
                _salutation = value;
                RaisePropertyChanged(nameof(Salutation));
            }
        }

        /// <summary>
        /// Binding property for the title
        /// </summary>
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

        /// <summary>
        /// opens the dialog to add a new title and refreshes the title list
        /// </summary>
        private void AddTitleCommandHandler()
        {
            if (!_dialogOpen)
            {
                _dialogOpen = true;

                AddTitleView _view = new AddTitleView();
                _view.DataContext = new AddTitleViewModel(_view, _apiClient);
                _view.ShowDialog();

                Task.Run(async () =>
                {
                    //refresh
                    var titles = await _apiClient.GetTitles();
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        AvailableTitles.Clear();
                        AvailableTitles.AddRange(titles);
                    });

                    _dialogOpen = false;
                });
            }
        }

        /// <summary>
        /// calls the api to parse the input, then opens the dialog to verify the parsed data
        /// </summary>
        private void ButtonParseHandler()
        {
            if (!_dialogOpen)
            {
                _dialogOpen = true;
                Task.Run(async () =>
                {
                    var result = await _apiClient.Parse(Input);

                    ErrorMessage = string.Join(", ", result.ErrorMessages.Select(x => x.Message));

                    if (result.StructuredName is not null)
                    {
                        if (result.StructuredName.Titles != null)
                            Titles = string.Join(", ", result.StructuredName.Titles);

                        Gender = result.StructuredName.Gender;
                        FirstName = result.StructuredName.FirstName;
                        LastName = result.StructuredName.LastName;
                        _key = Guid.NewGuid();
                    }

                    //der dispatcher-thread wird benötigt, um die GUI anpassen zu können
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        if (ErrorMessage.Contains("Es konnte keine Verbindung hergestellt werden, da der Zielcomputer die Verbindung verweigerte"))
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

        /// <summary>
        /// restes the list
        /// </summary>
        private void ButtonResetHandler() =>
            EnteredElements.Clear();

        /// <summary>
        /// converts the gender Enum to string values
        /// </summary>
        /// <param name="gender"></param>
        /// <returns> gender as string</returns>
        private string ConvertGenderEnumToString( GenderEnum gender )
        {
            return gender switch
            {
                GenderEnum.MALE => "Männlich",
                GenderEnum.FEMALE => "Weiblich",
                GenderEnum.DIVERSE => "Divers",
                _ => "Unbekannt",
            };
        }

        /// <summary>
        /// opens the parsedelements view
        /// </summary>
        /// <param name="parseResponse"></param>
        /// <param name="manuallyOpened"></param>
        private void OpenParsedElementsView( ParseResponseDto parseResponse, bool manuallyOpened = false )
        {
            ParsedElements _parsedView = new ParsedElements(_eventAggregator);
            _parsedView.DataContext = new ParsedElementsViewModel(_apiClient, _eventAggregator, _parsedView, parseResponse, manuallyOpened, Input);
            _parsedView.ShowDialog();
        }

        /// <summary>
        /// updates the parsed elements title and replaces the passed element
        /// </summary>
        /// <param name="updatedElement"></param>
        private void UpdateParsedElementsList( StructuredName updatedElement )
        {
            bool listContainsElement = EnteredElements.Any(element => element.Key == updatedElement.Key);
            updatedElement.GenderString = ConvertGenderEnumToString(updatedElement.Gender);

            if (listContainsElement)
                EnteredElements.Remove(EnteredElements.Where(element => element.Key == updatedElement.Key).Single());

            EnteredElements.Add(updatedElement);
        }

        /// <summary>
        /// Contsructor recives IApiClient and IEventAggregator via dependency injection.
        /// Delegate commands and AvailableTitles list get initialized.
        /// </summary>
        /// <param name="apiClient"></param>
        /// <param name="eventAggregator"></param>
        public SplitterViewModel( IApiClient apiClient, IEventAggregator eventAggregator )
        {
            _apiClient = apiClient;
            _eventAggregator = eventAggregator;

            ButtonParse = new DelegateCommand(ButtonParseHandler);
            ButtonReset = new DelegateCommand(ButtonResetHandler);
            AddTitleCommand = new DelegateCommand(AddTitleCommandHandler);
            ButtonAddManually = new DelegateCommand(() => OpenParsedElementsView(
                new ParseResponseDto
                {
                    StructuredName = new StructuredName
                    {
                        Key = Guid.NewGuid()
                    }
                }, true));

            _eventAggregator.GetEvent<ParseEvent>().Subscribe(ButtonParseHandler);
            _eventAggregator.GetEvent<UpdateParsedList>().Subscribe(UpdateParsedElementsList);
            _eventAggregator.GetEvent<OpenParsedElementsView>().Subscribe(( value ) => OpenParsedElementsView(value));

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
}