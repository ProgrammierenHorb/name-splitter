using NameSplitter.Services;
using Prism.Commands;
using Prism.Mvvm;
using System.Threading.Tasks;

namespace NameSplitter.ViewModels
{
    public class SplitterViewModel: BindableBase
    {
        #region privateVariables

        private IApiClient _apiClient;
        private bool _error = false;
        private string _errorMessage = string.Empty;
        private string _firstname = "";
        private string _gender = "";
        private string _input = "";
        private bool _isErrorChanged = false;
        private bool _isErrorMessageChanged = false;
        private bool _isFirstNameChanged = false;
        private bool _isGenderChanged = false;
        private bool _isLastNameChanged = false;
        private bool _isSalutationChanged = false;
        private bool _isStandardizedSalutationChanged = false;
        private bool _isTitleChanged = false;
        private string _salutation = "";
        private string _standardizedSalutation = "";
        private string _surname = "";
        private string _titles = "";

        #endregion privateVariables

        #region Properties

        public DelegateCommand ButtonParse { get; set; }

        public bool Error
        {
            get { return _error; }
            set
            {
                if( !_error.Equals(value) )
                {
                    IsErrorChanged = true;
                }
                _error = value;
                RaisePropertyChanged(nameof(Error));
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                if( !_errorMessage.Equals(value) )
                {
                    IsErrorMessageChanged = true;
                }
                _errorMessage = value;
                RaisePropertyChanged(nameof(_errorMessage));
            }
        }

        public string FirstName
        {
            get { return _firstname; }
            set
            {
                if( !_firstname.Equals(value) )
                {
                    IsFirstNameChanged = true;
                }
                _firstname = value;
                RaisePropertyChanged(nameof(FirstName));
            }
        }

        public string Gender
        {
            get { return _gender; }
            set
            {
                if( !_gender.Equals(value) )
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

        public bool IsErrorChanged
        {
            get { return _isErrorChanged; }
            set
            {
                _isErrorChanged = value;
                RaisePropertyChanged(nameof(IsErrorChanged));
            }
        }

        public bool IsErrorMessageChanged
        {
            get { return _isErrorMessageChanged; }
            set
            {
                _isErrorMessageChanged = value;
                RaisePropertyChanged(nameof(IsErrorMessageChanged));
            }
        }

        public bool IsFirstNameChanged
        {
            get { return _isFirstNameChanged; }
            set
            {
                _isFirstNameChanged = value;
                RaisePropertyChanged(nameof(IsFirstNameChanged));
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

        public bool IsLastNameChanged
        {
            get { return _isLastNameChanged; }
            set
            {
                _isLastNameChanged = value;
                RaisePropertyChanged(nameof(IsLastNameChanged));
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

        public bool IsStandardizedSalutationChanged
        {
            get { return _isStandardizedSalutationChanged; }
            set
            {
                _isStandardizedSalutationChanged = value;
                RaisePropertyChanged(nameof(IsStandardizedSalutationChanged));
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

        public string LastName
        {
            get { return _surname; }
            set
            {
                if( !_surname.Equals(value) )
                {
                    IsLastNameChanged = true;
                }
                _surname = value;
                RaisePropertyChanged(nameof(LastName));
            }
        }

        public string Salutation
        {
            get { return _salutation; }
            set
            {
                if( !_salutation.Equals(value) )
                {
                    IsSalutationChanged = true;
                }
                _salutation = value;
                RaisePropertyChanged(nameof(Salutation));
            }
        }

        public string StandardizedSalutation
        {
            get { return _standardizedSalutation; }
            set
            {
                if( !_standardizedSalutation.Equals(value) )
                {
                    IsStandardizedSalutationChanged = true;
                }
                _standardizedSalutation = value;
                RaisePropertyChanged(nameof(StandardizedSalutation));
            }
        }

        public string Titles
        {
            get { return _titles; }
            set
            {
                if( !_titles.Equals(value) )
                {
                    IsTitleChanged = true;
                }
                _titles = value;
                RaisePropertyChanged(nameof(Titles));
            }
        }

        #endregion Properties

        public SplitterViewModel( IApiClient apiClient )
        {
            _apiClient = apiClient;
            ButtonParse = new DelegateCommand(ButtonParseHandler);
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
                //Salutation = result.Structuredname.StandardizedSalutation;
            });
        }
    }
}