using NameSplitter.DTOs;
using NameSplitter.Enum;
using NameSplitter.Events;
using NameSplitter.Services;
using NameSplitter.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace NameSplitter.ViewModels
{
    internal class ParsedElementsViewModel: BindableBase
    {
        public DelegateCommand ButtonCancle { get; set; }
        public DelegateCommand ButtonSave { get; set; }

        #region private variables

        private IApiClient _apiClient;
        private bool _diversIsChecked = false;
        private IEventAggregator _eventAggregator;
        private bool _femaleIsChecked = false;
        private bool _maleIsChecked = false;
        private ParseResponseDto _parsedElement;
        private ParsedElements _parsedElementsView;
        private string _responseText;

        #endregion private variables

        #region Properties

        public bool DiversIsChecked
        {
            get { return _diversIsChecked; }
            set
            {
                _diversIsChecked = value;
                RaisePropertyChanged(nameof(DiversIsChecked));
            }
        }

        public bool FemaleIsChecked
        {
            get { return _femaleIsChecked; }
            set
            {
                _femaleIsChecked = value;
                RaisePropertyChanged(nameof(FemaleIsChecked));
            }
        }

        public bool MaleIsChecked
        {
            get { return _maleIsChecked; }
            set
            {
                _maleIsChecked = value;
                RaisePropertyChanged(nameof(MaleIsChecked));
            }
        }

        public ParseResponseDto ParsedItems
        {
            get { return _parsedElement; }
            set
            {
                _parsedElement = value;
                RaisePropertyChanged(nameof(ParsedItems));
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

        #endregion Properties

        public ParsedElementsViewModel( IApiClient apiClient, IEventAggregator eventAggregator, ParsedElements parsedElementsView, ParseResponseDto parsedElement )
        {
            _apiClient = apiClient;
            _eventAggregator = eventAggregator;
            _parsedElementsView = parsedElementsView;
            _responseText = "Test";

            ParsedItems = parsedElement;

            ButtonSave = new DelegateCommand(SaveParsedElementsButtonHandler);
            ButtonCancle = new DelegateCommand(CancleButtonHandler);

            InitializeRadioButtons(ParsedItems.StructuredName?.Gender ?? GenderEnum.DIVERSE);
        }

        public void CancleButtonHandler() =>
            _parsedElementsView.Close();

        public void InitializeRadioButtons( GenderEnum gender )
        {
            _diversIsChecked = false;
            _maleIsChecked = false;
            _femaleIsChecked = false;

            switch( gender )
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
            //TODO: SplitterViewModel Properties updaten;
            bool successful = await _apiClient.SaveParsedElement(ParsedItems.StructuredName);
            if( successful )
                _eventAggregator.GetEvent<UpdateParsedList>().Publish(ParsedItems.StructuredName);

            _parsedElementsView.Close();
        }
    }
}