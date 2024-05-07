using NameSplitter.DTOs;
using NameSplitter.Events;
using Prism.Events;
using System.Windows.Controls;
using System.Windows.Input;

namespace NameSplitter.Views
{
    /// <summary>
    /// Interaction logic for SplitterView.xaml
    /// </summary>
    public partial class SplitterView : UserControl
    {
        private IEventAggregator _eventAggregator;

        private void ListView_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            if (EnteredElementsListView.SelectedItem != null)
                _eventAggregator.GetEvent<OpenParsedElementsView>().Publish(
                    new ParseResponseDto
                    {
                        StructuredName = EnteredElementsListView.SelectedItem as StructuredName
                    }
                );
        }

        private void Window_KeyUp( object sender, KeyEventArgs e )
        {
            if (e.Key == Key.Enter)
            {
                _eventAggregator.GetEvent<ParseEvent>().Publish();
            }
        }

        public SplitterView( IEventAggregator eventAggregator )
        {
            _eventAggregator = eventAggregator;
            InitializeComponent();
        }
    }
}