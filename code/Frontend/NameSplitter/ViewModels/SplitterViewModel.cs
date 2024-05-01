using NameSplitter.Services;
using Prism.Commands;
using Prism.Mvvm;

namespace NameSplitter.ViewModels
{
    public class SplitterViewModel : BindableBase
    {
        #region privateVariables

        private IApiClient _apiClient;
        private string _firstname = "";
        private string _gender = "";
        private string _input = "";
        private bool _isFirstnameChanged = false;
        private bool _isGenderChanged = false;
        private bool _isSalutationChanged = false;
        private bool _isStandardizedLetterSalutationChanged = false;
        private bool _isSurnameChanged = false;
        private bool _isTitleChanged = false;
        private string _salutation = "";
        private string _standardizedLetterSalutation = "";
        private string _surname = "";
        private string _title = "";

        #endregion privateVariables

        #region Properties

        public DelegateCommand ButtonParse { get; set; }

        public string Firstname
        {
            get { return _firstname; }
            set
            {
                if (!_firstname.Equals(value))
                {
                    IsFirstnameChanged = true;
                }
                _firstname = value;
                RaisePropertyChanged(nameof(Firstname));
            }
        }

        public string Gender
        {
            get { return _gender; }
            set
            {
                if (!_gender.Equals(value))
                {
                    IsGenderChanged = true;
                }
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

        public bool IsFirstnameChanged
        {
            get { return _isFirstnameChanged; }
            set
            {
                _isFirstnameChanged = value;
                RaisePropertyChanged(nameof(IsFirstnameChanged));
            }
        }

        public bool IsGenderChanged
        {
            get { return _isGenderChanged; }
            set
            {
                _isGenderChanged = value;
                RaisePropertyChanged(nameof(IsGenderChanged));
            }
        }

        public bool IsSalutationChanged
        {
            get { return _isSalutationChanged; }
            set
            {
                _isSalutationChanged = value;
                RaisePropertyChanged(nameof(IsSalutationChanged));
            }
        }

        public bool IsStandardizedLetterSalutationChanged
        {
            get { return _isStandardizedLetterSalutationChanged; }
            set
            {
                _isStandardizedLetterSalutationChanged = value;
                RaisePropertyChanged(nameof(IsStandardizedLetterSalutationChanged));
            }
        }

        public bool IsSurnameChanged
        {
            get { return _isSurnameChanged; }
            set
            {
                _isSurnameChanged = value;
                RaisePropertyChanged(nameof(IsSurnameChanged));
            }
        }

        public bool IsTitleChanged
        {
            get { return _isTitleChanged; }
            set
            {
                _isTitleChanged = value;
                RaisePropertyChanged(nameof(IsTitleChanged));
            }
        }

        public string Salutation
        {
            get { return _salutation; }
            set
            {
                if (!_salutation.Equals(value))
                {
                    IsSalutationChanged = true;
                }
                _salutation = value;
                RaisePropertyChanged(nameof(Salutation));
            }
        }

        public string StandardizedLetterSalutation
        {
            get { return _standardizedLetterSalutation; }
            set
            {
                if (!_standardizedLetterSalutation.Equals(value))
                {
                    IsStandardizedLetterSalutationChanged = true;
                }
                _standardizedLetterSalutation = value;
                RaisePropertyChanged(nameof(StandardizedLetterSalutation));
            }
        }

        public string Surname
        {
            get { return _surname; }
            set
            {
                if (!_surname.Equals(value))
                {
                    IsSurnameChanged = true;
                }
                _surname = value;
                RaisePropertyChanged(nameof(Surname));
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                if (!_title.Equals(value))
                {
                    IsTitleChanged = true;
                }
                _title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        #endregion Properties

        private void ButtonParseHandler()
        {
            var result = _apiClient.Parse(Input);

            Title = result.Title;
            Salutation = result.Salutation;
            StandardizedLetterSalutation = result.StandardizedLetterSalutation;
            Gender = result.Gender;
            Firstname = result.Firstname;
            Surname = result.Surname;

            Title = Input;
            Surname = Input;
        }

        public SplitterViewModel( IApiClient apiClient )
        {
            _apiClient = apiClient;
            ButtonParse = new DelegateCommand(ButtonParseHandler);
        }
    }
}