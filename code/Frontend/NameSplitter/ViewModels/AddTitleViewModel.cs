using NameSplitter.DTOs;
using NameSplitter.Enum;
using NameSplitter.Services;
using NameSplitter.Views;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;

namespace NameSplitter.ViewModels
{
    /// <summary>
    /// Viewmodel for AddTitle view,
    /// used to add new titles
    /// </summary>
    public class AddTitleViewModel: BindableBase
    {
        public DelegateCommand AddTitleCommand { get; set; }
        public DelegateCommand ButtonCancle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [diverse is checked].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [diverse is checked]; otherwise, <c>false</c>.
        /// </value>
        public bool DiverseIsChecked
        {
            get { return _diverseIsChecked; }
            set
            {
                _diverseIsChecked = value;
                RaisePropertyChanged(nameof(DiverseIsChecked));
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [female is checked].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [female is checked]; otherwise, <c>false</c>.
        /// </value>
        public bool FemaleIsChecked
        {
            get { return _femaleIsChecked; }
            set
            {
                _femaleIsChecked = value;
                RaisePropertyChanged(nameof(FemaleIsChecked));
            }
        }

        /// <summary>
        /// Binding property for new title
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// Binding property for new regex of the title
        /// </summary>
        public string InputRegex { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [male is checked].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [male is checked]; otherwise, <c>false</c>.
        /// </value>
        public bool MaleIsChecked
        {
            get { return _maleIsChecked; }
            set
            {
                _maleIsChecked = value;
                RaisePropertyChanged(nameof(MaleIsChecked));
            }
        }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public int Priority { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [unknown is checked].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [unknown is checked]; otherwise, <c>false</c>.
        /// </value>
        public bool UnknownIsChecked
        {
            get { return _unknownIsChecked; }
            set
            {
                _unknownIsChecked = value;
                RaisePropertyChanged(nameof(UnknownIsChecked));
            }
        }

        /// <summary>
        /// Binding property for the checkbox
        /// </summary>
        public bool UseRegex
        {
            get
            {
                return _useRegex;
            }
            set
            {
                _useRegex = value;
                RaisePropertyChanged(nameof(Visibility));
            }
        }

        /// <summary>
        /// Binding property for the visibility of the regex textbox
        /// </summary>
        public Visibility Visibility
        {
            get
            {
                if( UseRegex ) return Visibility.Visible;
                return Visibility.Hidden;
            }
        }

        private IApiClient _apiClient;
        private bool _diverseIsChecked = false;
        private bool _femaleIsChecked = false;
        private GenderEnum? _gender = null;
        private bool _maleIsChecked = false;

        private bool _unknownIsChecked = true;
        private bool _useRegex = false;

        private AddTitleView _window;

        /// <summary>
        /// constructor and initailsation of delegate commands
        /// </summary>
        /// <param name="window"></param>
        /// <param name="apiClient"></param>
        public AddTitleViewModel( AddTitleView window, IApiClient apiClient )
        {
            _window = window;
            _apiClient = apiClient;
            AddTitleCommand = new DelegateCommand(AddTitleCommandHandlerAsync);

            ButtonCancle = new DelegateCommand(CancleButtonHandler);
        }

        /// <summary>
        /// Command to save the new title in backend using the api
        /// </summary>
        public async void AddTitleCommandHandlerAsync()
        {
            string gender;
            string response;

            if(string.IsNullOrEmpty(Input) )
            {
                MessageBox.Show("Sie müssen einen Titel eingeben", "Titel eingeben", MessageBoxButton.OK,
                                MessageBoxImage.Error);
                return; 
            }

            if( _maleIsChecked ) gender = "MALE";
            else if( _femaleIsChecked ) gender = "FEMALE";
            else if( _diverseIsChecked ) gender = "DIVERSE";
            else gender = null;

            if( UseRegex )
                response = await _apiClient.AddTitle(new Title { Name = Input, Priority = Priority, GenderString = gender, Regex = InputRegex });
            else
                response = await _apiClient.AddTitle(new Title { Name = Input, Priority = Priority, GenderString = gender, Regex = null });
            if( response.Equals("success") )
            {
                _window.Close();
            }
            else if( response.Equals("already present") )
            {
                MessageBox.Show($"\"{Input}\" ist bereits innerhalb der Titel-Datenbank vorhanden. Bitte verwenden Sie einen anderen Namen", "Titel bereits vorhanden", MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
            else if( response.Equals("server not reachable") )
            {
                MessageBox.Show("Der Server konnte nicht erreicht werden \nBitte überprüfen Sie, ob das Backend gestartert wurde. " +
                             "Den Status können Sie unter http://localhost:8080/api/status abfragen.", "Keine Verbindung zum Server möglich", MessageBoxButton.OK,
                             MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Cancel command to close the window
        /// </summary>
        public void CancleButtonHandler()
        { _window.Close(); }
    }
}