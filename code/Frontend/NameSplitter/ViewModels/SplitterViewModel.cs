using NameSplitter.Services;
using NameSplitter.Views;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NameSplitter.ViewModels
{
    public class SplitterViewModel : BindableBase
    {
        private string _title;

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }

        private string _salutation;

        public string Salutation
        {
            get { return _salutation; }
            set
            {
                _salutation = value;
                RaisePropertyChanged(nameof(Salutation));
            }
        }

        private string _standardizedLetterSalutation;

        public string StandardizedLetterSalutation
        {
            get { return _standardizedLetterSalutation; }
            set
            {
                _standardizedLetterSalutation = value;
                RaisePropertyChanged(nameof(StandardizedLetterSalutation));
            }
        }

        private string _gender;

        public string Gender
        {
            get { return _gender; }
            set
            {
                _gender = value;
                RaisePropertyChanged(nameof(Gender));
            }
        }

        private string _firstname;

        public string Firstname
        {
            get { return _firstname; }
            set
            {
                _firstname = value;
                RaisePropertyChanged(nameof(Firstname));
            }
        }

        private string _surname;

        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                RaisePropertyChanged(nameof(Surname));
            }
        }

        private string _input;

        public string Input
        {
            get { return _input; }
            set
            {
                _input = value;
                RaisePropertyChanged(nameof(Input));
            }
        }

        private IApiClient _apiClient;

        private SplitterViewModel( IApiClient apiClient )
        {
            _apiClient = apiClient;
            ButtonParse = new DelegateCommand(ButtonParseHandler);
        }

        private void ButtonParseHandler()
        {
            var result = _apiClient.Parse(Input);
        }

        public DelegateCommand ButtonParse { get; set; }
    }
}