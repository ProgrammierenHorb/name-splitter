using NameSplitter.DTOs;
using NameSplitter.Enum;
using NameSplitter.Events;
using NameSplitter.Services;
using NameSplitter.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace NameSplitter.ViewModels
{
    /// <summary>
    /// Viewmodel for ParsedElements view,
    /// used to display and validate the result of the parser
    /// </summary>
    public class ParsedElementsViewModel : BindableBase
    {
        /// <summary>
        /// initializes the textbox values
        /// </summary>
        /// <param name="parseResponse"></param>
        private void InitView( ParseResponseDto parseResponse )
        {
            if (parseResponse is null)
            {
                ResponseText.Add(new TextWithColor("Es ist ein Fehler aufgetreten. Bitte parsen Sie Ihre Eingabe erneut.", new SolidColorBrush(Colors.Red)));
                return;
            }

            if (parseResponse.StructuredName is null)
                return;

            if (parseResponse.StructuredName.Titles is not null)
            {
                foreach (var title in parseResponse.StructuredName.Titles)
                {
                    Titles.Add(new Title { Name = title });
                }
            }

            FirstName = parseResponse.StructuredName.FirstName;
            LastName = parseResponse.StructuredName.LastName;
            Gender = parseResponse.StructuredName?.Gender ?? GenderEnum.DIVERSE;

            InitRadioButtons();
        }

        /// <summary>
        /// constructor, which sets up the descriptive label in the view and the delegate commands
        /// </summary>
        /// <param name="apiClient"></param>
        /// <param name="eventAggregator"></param>
        /// <param name="parsedElementsView"></param>
        /// <param name="parsedElement"></param>
        /// <param name="manuallyOpened"></param>
        public ParsedElementsViewModel( IApiClient apiClient, IEventAggregator eventAggregator, ParsedElementsView parsedElementsView, ParseResponseDto parsedElement, bool manuallyOpened, string input )
        {
            _apiClient = apiClient;
            _eventAggregator = eventAggregator;
            _parsedElementsView = parsedElementsView;
            _key = parsedElement.StructuredName?.Key ?? Guid.NewGuid();
            Input = input;
            if (manuallyOpened)
            {
                _foregroundColor = "Green";
                _viewsTitle = "Elemente manuell eintragen";
                ResponseText.Add(new TextWithColor("Hier können Sie Ihre Angaben manuell eintragen, indem Sie die unten stehende Elemente anpassen.", new SolidColorBrush(Colors.Green)));
            }
            else
            {
                if (parsedElement.ErrorMessages != null && parsedElement.ErrorMessages.Any())
                {
                    _foregroundColor = "Red";
                    _viewsTitle = "Das Parsen ist fehlgeschlagen";
                }
                else
                {
                    _foregroundColor = "Green";
                    _viewsTitle = "Das Parsen war erfolgreich";
                }
                FormatResponseText(parsedElementsView, parsedElement);
            }

            ButtonSave = new DelegateCommand(SaveParsedElementsButtonHandler);
            ButtonCancle = new DelegateCommand(CancleButtonHandler);
            AddNewTitleCommand = new DelegateCommand(AddNewTitleCommandHandler);

            _eventAggregator.GetEvent<SaveParsedElements>().Subscribe(SaveParsedElementsButtonHandler);

            Task.Run(() =>
            {
                AllAvailableTitles = new ObservableCollection<string>(_apiClient.GetTitles().Result.DistinctBy(x => x.Name).Select(x => x.Name))
                {
                     "-Keine Auswahl-" // add placeholder
                };
            }).Wait();

            InitView(parsedElement);
        }

        public DelegateCommand AddNewTitleCommand { get; set; }

        public DelegateCommand ButtonCancle { get; set; }

        public DelegateCommand ButtonSave { get; set; }

        /// <summary>
        /// Adds another ComboBox inside the view.
        /// The initial value of it is "-Keine Auswahl-"
        /// </summary>
        public void AddNewTitleCommandHandler() =>
            Titles.Add(new Title { Name = "-Keine Auswahl-" });

        /// <summary>
        /// Cancel command to close the window
        /// </summary>
        public void CancleButtonHandler() =>
            _parsedElementsView.Close();

        /// <summary>
        /// If there are errors after parsing the users entered string, the string gets
        /// formatted by changing the colors at the position at which the error happened.
        /// </summary>
        /// <param name="parsedElementsView">The parsed elements view.</param>
        /// <param name="parsedElement">The parsed element.</param>
        public void FormatResponseText( ParsedElementsView parsedElementsView, ParseResponseDto parsedElement )
        {
            List<TextWithColor> inputTextWithColor = new();
            if (parsedElement.ErrorMessages == null || !parsedElement.ErrorMessages.Any())
            {
                inputTextWithColor.Add(new TextWithColor(Input, new SolidColorBrush(Colors.Black)));
                ResponseText.Add(new TextWithColor("Im Folgenden stehen alle gefundenen Elemente, welche Sie nun noch vor dem Speichern anpassen können.", new SolidColorBrush(Colors.Green)));
            }
            else
            {
                var errors = parsedElement.ErrorMessages.OrderBy(e => e.StartPos).ToList();
                string inputStr = Input;
                int currentPos = 0;
                if (!string.IsNullOrEmpty(inputStr))
                {
                    for (int i = 0; i < errors.Count; i++)
                    {
                        var error = errors[i];
                        if (currentPos < error.StartPos)
                        {
                            // add a black tuple up to error.StartPos
                            string str = inputStr.Substring(currentPos, error.StartPos - currentPos);
                            inputTextWithColor.Add(new TextWithColor(str, new SolidColorBrush(Colors.Black)));
                            currentPos = error.StartPos;
                        }

                        // now add colored tuple up to error.EndPos
                        int length = error.EndPos - currentPos;
                        if(length < 1)length = 1;
                        string errorStr = inputStr.Substring(currentPos, length);
                        var color = new SolidColorBrush((Color)ColorConverter.ConvertFromString(GetRandomHexColor()));
                        inputTextWithColor.Add(new TextWithColor(errorStr, color));
                        ResponseText.Add(new TextWithColor(error.Message, color));

                        currentPos = error.EndPos + 1;
                    }

                    // if there's any string left after last error, add it in black
                    if (currentPos < inputStr.Length)
                    {
                        string endStr = inputStr.Substring(currentPos);
                        inputTextWithColor.Add(new TextWithColor(endStr, new SolidColorBrush(Colors.Black)));
                    }
                }
                else
                {
                    inputTextWithColor.Add(new TextWithColor("Keine Eingabe vorhanden", new SolidColorBrush(Colors.Red)));
                }
            }
            parsedElementsView.WriteInTextbox(inputTextWithColor);
        }

        /// <summary>
        /// Gets the random color of the hexadecimal.
        /// </summary>
        /// <returns>color as a string</returns>
        public string GetRandomHexColor()
        {
            Random random = new Random();
            return String.Format("#{0:X6}", random.Next(0x1000000)); // generates a random color
        }

        /// <summary>
        /// initialisation of the radio buttons
        /// </summary>
        public void InitRadioButtons()
        {
            _diversIsChecked = false;
            _maleIsChecked = false;
            _femaleIsChecked = false;

            switch (Gender)
            {
                case GenderEnum.MALE:
                    _maleIsChecked = true;
                    break;

                case GenderEnum.FEMALE:
                    _femaleIsChecked = true;
                    break;

                case GenderEnum.DIVERSE:
                    _diversIsChecked = true;
                    break;
            }
        }

        /// <summary>
        /// saves the parsed elements in the backend using the api
        /// </summary>
        public async void SaveParsedElementsButtonHandler()
        {
            if (string.IsNullOrEmpty(LastName))
            {
                MessageBox.Show("Bitte geben Sie mindestens einen Nachnamen ein", "Keine valide Angaben", MessageBoxButton.OK,
                MessageBoxImage.Error);
                return;
            }

            StructuredName structuredName = new StructuredName
            {
                Titles = Titles.Where(x => x.Name != "-Keine Auswahl-").Select(element => element.Name).ToList() ?? new List<string>(),
                FirstName = this.FirstName ?? null,
                LastName = this.LastName ?? "Unbekannt",
                GenderString = this.Gender.ToString() ?? GenderEnum.DIVERSE.ToString(),
                Key = _key
            };

            var adjustedStructuredName = await _apiClient.SaveParsedElement(structuredName);
            if (adjustedStructuredName is not null)
                _eventAggregator.GetEvent<UpdateParsedList>().Publish(adjustedStructuredName);

            _parsedElementsView.Close();
        }

        #region private variables

        private ObservableCollection<string> _allAvailableTitles;
        private IApiClient _apiClient;
        private bool _diversIsChecked = true;
        private IEventAggregator _eventAggregator;
        private bool _femaleIsChecked = false;
        private string _firstName;
        private string _foregroundColor = "Red";
        private GenderEnum _gender = GenderEnum.DIVERSE;
        private string _input;
        private Guid _key;
        private string _lastName;
        private bool _maleIsChecked = false;
        private ParsedElementsView _parsedElementsView;
        private List<TextWithColor> _responseText = new();
        private ObservableCollection<Title> _titles = new ObservableCollection<Title>();
        private string _viewsTitle = "Das Parsen ist fehlgeschlagen";

        #endregion private variables

        #region Properties

        /// <summary>
        /// ItemSource for all available titles
        /// </summary>
        public ObservableCollection<string> AllAvailableTitles
        {
            get
            {
                return _allAvailableTitles;
            }
            set
            {
                _allAvailableTitles = value;
                RaisePropertyChanged(nameof(AllAvailableTitles));
            }
        }

        /// <summary>
        /// Binding property for the divers radio button
        /// </summary>
        public bool DiversIsChecked
        {
            get { return _diversIsChecked; }
            set
            {
                _diversIsChecked = value;
                if (_diversIsChecked) _gender = GenderEnum.DIVERSE;
                RaisePropertyChanged(nameof(DiversIsChecked));
            }
        }

        /// <summary>
        /// Binding property for the female radio button
        /// </summary>
        public bool FemaleIsChecked
        {
            get { return _femaleIsChecked; }
            set
            {
                _femaleIsChecked = value;
                if (_femaleIsChecked) _gender = GenderEnum.FEMALE;
                RaisePropertyChanged(nameof(FemaleIsChecked));
            }
        }

        /// <summary>
        /// Binding property for firstname
        /// </summary>
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }

        /// <summary>
        /// Binding property for foregroundcolor of label
        /// </summary>
        public string ForegroundColor
        {
            get { return _foregroundColor; }
            set
            {
                _foregroundColor = value;
                RaisePropertyChanged(nameof(ForegroundColor));
            }
        }

        /// <summary>
        /// Prpoerty for Gender
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
        /// Property for Input
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
        /// Binding property for lastname
        /// </summary>
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                RaisePropertyChanged(nameof(LastName));
            }
        }

        /// <summary>
        /// Binding property for the male radio button
        /// </summary>
        public bool MaleIsChecked
        {
            get { return _maleIsChecked; }
            set
            {
                _maleIsChecked = value;
                if (_maleIsChecked) _gender = GenderEnum.MALE;
                RaisePropertyChanged(nameof(MaleIsChecked));
            }
        }

        /// <summary>
        /// Binding property for responsetext
        /// </summary>
        public List<TextWithColor> ResponseText
        {
            get { return _responseText; }
            set
            {
                _responseText = value;
                RaisePropertyChanged(nameof(ResponseText));
            }
        }

        /// <summary>
        /// Binding property for Titles
        /// </summary>
        public ObservableCollection<Title> Titles
        {
            get
            {
                return _titles;
            }
            set
            {
                _titles = value;
                RaisePropertyChanged(nameof(Titles));
            }
        }

        /// <summary>
        /// Binding property for title of the view
        /// </summary>
        public string ViewsTitle
        {
            get { return _viewsTitle; }
            set
            {
                _viewsTitle = value;
                RaisePropertyChanged(nameof(ViewsTitle));
            }
        }

        #endregion Properties
    }
}