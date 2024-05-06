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
using System.Linq;

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
                _responseText = "Es ist ein Fehler aufgetreten. Bitte parsen Sie Ihre Eingabe erneut.";
                return;
            }

            if (parseResponse.StructuredName is null)
                return;

            if (parseResponse.StructuredName.Titles is not null)
                TitlesAsString = string.Join(", ", parseResponse.StructuredName.Titles);

            FirstName = parseResponse.StructuredName.FirstName;
            LastName = parseResponse.StructuredName.LastName;
            StandardizedSalutation = parseResponse.StructuredName.StandardizedSalutation;
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
        public ParsedElementsViewModel( IApiClient apiClient, IEventAggregator eventAggregator, ParsedElements parsedElementsView, ParseResponseDto parsedElement, bool manuallyOpened )
        {
            _apiClient = apiClient;
            _eventAggregator = eventAggregator;
            _parsedElementsView = parsedElementsView;
            _key = parsedElement.StructuredName?.Key ?? Guid.NewGuid();
            if (manuallyOpened)
            {
                _foregroundColor = "Green";
                _viewsTitle = "Elemente manuell eintragen";
                _responseText = "Hier können SIe Ihre Angaben manuell eintragen, indem Sie die unten stehende Elemente anpassen.";
            }
            else
            {
                _foregroundColor = parsedElement.Error ? "Red" : "Green";
                _viewsTitle = parsedElement.Error ? "Das Parsen ist fehlgeschlagen" : "Das Parsen war erfolgreich";
                _responseText = parsedElement.Error ?
                   parsedElement.ErrorMessage :
                   "Im Folgenden stehen alle gefundenen Elemente, welche Sie nun noch vor dem Speichern anpassen können.";
            }

            ButtonSave = new DelegateCommand(SaveParsedElementsButtonHandler);
            ButtonCancle = new DelegateCommand(CancleButtonHandler);

            _eventAggregator.GetEvent<SaveParsedElements>().Subscribe(SaveParsedElementsButtonHandler);

            InitView(parsedElement);
        }

        public DelegateCommand ButtonCancle { get; set; }
        public DelegateCommand ButtonSave { get; set; }

        #region private variables

        private IApiClient _apiClient;
        private bool _diversIsChecked = true;
        private IEventAggregator _eventAggregator;
        private bool _femaleIsChecked = false;
        private string _firstName;
        private string _foregroundColor = "Red";
        private GenderEnum _gender = GenderEnum.DIVERSE;
        private Guid _key;
        private string _lastName;
        private bool _maleIsChecked = false;
        private ParsedElements _parsedElementsView;
        private string _responseText;
        private string _standardizedSalutation;
        private string _titlesAsString;
        private string _viewsTitle = "Das Parsen ist fehlgeschlagen";

        #endregion private variables

        #region Properties

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
        public string ResponseText
        {
            get { return _responseText; }
            set
            {
                _responseText = value;
                RaisePropertyChanged(nameof(ResponseText));
            }
        }

        /// <summary>
        /// Binding property for standardized salutation
        /// </summary>
        public string StandardizedSalutation
        {
            get { return _standardizedSalutation; }
            set
            {
                _standardizedSalutation = value;
                RaisePropertyChanged(nameof(StandardizedSalutation));
            }
        }

        /// <summary>
        /// Binding property for title
        /// </summary>
        public string TitlesAsString
        {
            get { return _titlesAsString; }
            set
            {
                _titlesAsString = value;
                RaisePropertyChanged(nameof(TitlesAsString));
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

        /// <summary>
        /// Cancel command to close the window
        /// </summary>
        public void CancleButtonHandler() =>
            _parsedElementsView.Close();

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
            StructuredName structuredName = new StructuredName
            {
                Titles = TitlesAsString?.Split(',').Where(element => element is not "" && element is not " ").ToList() ?? new List<string>(),
                FirstName = this.FirstName ?? "Unbekannt",
                LastName = this.LastName ?? "Unbekannt",
                GenderString = this.Gender.ToString() ?? GenderEnum.DIVERSE.ToString(),
                StandardizedSalutation = this.StandardizedSalutation ?? "Unbekannt",
                Key = _key
            };

            if (await _apiClient.SaveParsedElement(structuredName))
                _eventAggregator.GetEvent<UpdateParsedList>().Publish(structuredName);

            _parsedElementsView.Close();
        }
    }
}