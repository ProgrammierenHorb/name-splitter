using NameSplitter.DTOs;
using NameSplitter.Enum;
using NameSplitter.Events;
using NameSplitter.Services;
using NameSplitter.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;

namespace NameSplitter.ViewModels
{
    public class ParsedElementsViewModel: BindableBase
    {
        public DelegateCommand ButtonCancle { get; set; }
        public DelegateCommand ButtonSave { get; set; }

        #region private variables

        private IApiClient _apiClient;
        private bool _diversIsChecked = false;
        private IEventAggregator _eventAggregator;
        private bool _femaleIsChecked = false;
        private string _firstName;
        private GenderEnum _gender;
        private string _lastName;
        private bool _maleIsChecked = false;
        private ParsedElements _parsedElementsView;
        private string _responseText;
        private string _standardizedSalutation;

        #endregion private variables

        #region Properties

        public bool DiversIsChecked
        {
            get { return _diversIsChecked; }
            set
            {
                _diversIsChecked = value;
                if( _diversIsChecked ) _gender = GenderEnum.DIVERSE;
                RaisePropertyChanged(nameof(DiversIsChecked));
            }
        }

        public bool FemaleIsChecked
        {
            get { return _femaleIsChecked; }
            set
            {
                _femaleIsChecked = value;
                if( _femaleIsChecked ) _gender = GenderEnum.FEMALE;
                RaisePropertyChanged(nameof(FemaleIsChecked));
            }
        }

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
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

        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                RaisePropertyChanged(nameof(LastName));
            }
        }

        public bool MaleIsChecked
        {
            get { return _maleIsChecked; }
            set
            {
                _maleIsChecked = value;
                if( _maleIsChecked ) _gender = GenderEnum.MALE;
                RaisePropertyChanged(nameof(MaleIsChecked));
            }
        }

        public string ResponseText
        {
            get { return _responseText; }
            set
            {
                _responseText = value;
                RaisePropertyChanged(nameof(ResponseText));
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

        public ObservableCollection<TitleDto> Titles { get; set; } = new ObservableCollection<TitleDto>();

        #endregion Properties

        public ParsedElementsViewModel( IApiClient apiClient, IEventAggregator eventAggregator, ParsedElements parsedElementsView, ParseResponseDto parsedElement )
        {
            _apiClient = apiClient;
            _eventAggregator = eventAggregator;
            _parsedElementsView = parsedElementsView;

            ButtonSave = new DelegateCommand(SaveParsedElementsButtonHandler);
            ButtonCancle = new DelegateCommand(CancleButtonHandler);

            InitView(parsedElement);
        }

        public void CancleButtonHandler() =>
            _parsedElementsView.Close();

        public void InitRadioButtons()
        {
            _diversIsChecked = false;
            _maleIsChecked = false;
            _femaleIsChecked = false;

            switch( Gender )
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

        public async void SaveParsedElementsButtonHandler()
        {
            StructuredName structuredName = new StructuredName
            {
                Titles = this.Titles.Select(x => x.Title).ToList(),
                FirstName = this.FirstName,
                LastName = this.LastName,
                GenderString = this.Gender.ToString(),
                StandardizedSalutation = this.StandardizedSalutation,
            };

            if( await _apiClient.SaveParsedElement(structuredName) )
                _eventAggregator.GetEvent<UpdateParsedList>().Publish(structuredName);

            _parsedElementsView.Close();
        }

        private void InitView( ParseResponseDto parseResponse )
        {
            if( parseResponse is null )
            {
                _responseText = "Es ist ein Fehler aufgetreten. Bitte parsen Sie Ihre Eingabe erneut.";
                return;
            }

            _responseText = parseResponse.Error ?
                parseResponse.ErrorMessage :
                "Im Folgenden stehen alle gefundenen Elemente, welche Sie nun noch vor dem Speichern anpassen können.";

            if( parseResponse.StructuredName is null )
                return;

            parseResponse.StructuredName.Titles.ForEach(title => Titles.Add(new TitleDto { Title = title }));
            FirstName = parseResponse.StructuredName.FirstName;
            LastName = parseResponse.StructuredName.LastName;
            StandardizedSalutation = parseResponse.StructuredName.StandardizedSalutation;
            Gender = parseResponse.StructuredName?.Gender ?? GenderEnum.DIVERSE;

            InitRadioButtons();
        }

        private void SetGender()
        {
        }
    }
}