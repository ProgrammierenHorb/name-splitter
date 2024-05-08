using Prism.Mvvm;

namespace NameSplitter.ViewModels
{
    /// <summary>
    /// Mainwindow, startingwindow of application
    /// </summary>
    public class MainWindowViewModel: BindableBase
    {
        /// <summary>
        /// title of mainwindow
        /// </summary>
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