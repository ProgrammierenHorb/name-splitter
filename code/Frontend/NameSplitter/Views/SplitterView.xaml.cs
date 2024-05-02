using NameSplitter.Events;
using Prism.Events;
using System.Windows.Controls;
using System.Windows.Input;

namespace NameSplitter.Views
{
    /// <summary>
    /// Interaction logic for SplitterView.xaml
    /// </summary>
    public partial class SplitterView: UserControl
    {
        private IEventAggregator _eventAggregator;

        public SplitterView( IEventAggregator eventAggregator )
        {
            _eventAggregator = eventAggregator;
            InitializeComponent();
        }

        private void Window_KeyUp( object sender, KeyEventArgs e )
        {
            if( e.Key == Key.Enter )
            {
                _eventAggregator.GetEvent<ParseEvent>().Publish();
            }
        }
    }
}