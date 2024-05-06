using NameSplitter.Events;
using Prism.Events;
using System.Windows;
using System.Windows.Input;

namespace NameSplitter.Views
{
    /// <summary>
    /// Interaction logic for ParsedElements.xaml
    /// </summary>
    public partial class ParsedElements: Window
    {
        private IEventAggregator _eventAggregator;

        public ParsedElements( IEventAggregator eventAggregator )
        {
            _eventAggregator = eventAggregator;
            InitializeComponent();
        }

        private void Window_KeyUp( object sender, KeyEventArgs e )
        {
            if( e.Key == Key.Enter )
                _eventAggregator.GetEvent<SaveParsedElements>().Publish();
        }
    }
}