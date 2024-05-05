using NameSplitter.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace NameSplitter.ViewModels
{
    public class MainWindowViewModel: BindableBase
    {
        public string Title

        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private string _title = "NameSplitter";

        public MainWindowViewModel()
        { }
    }
}