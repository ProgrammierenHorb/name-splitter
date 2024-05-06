using NameSplitter.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;

namespace NameSplitter.ViewModels
{
    /// <summary>
    /// Mainwindow, startingwindow of application
    /// </summary>
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "NameSplitter";

        public MainWindowViewModel()
        { }

        /// <summary>
        /// title of mainwindow
        /// </summary>
        public string Title

        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}