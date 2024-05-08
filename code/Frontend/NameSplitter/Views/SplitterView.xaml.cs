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
    public partial class SplitterView: UserControl
    {
        private IEventAggregator _eventAggregator;

        public SplitterView( IEventAggregator eventAggregator )
        {
            _eventAggregator = eventAggregator;
            InitializeComponent();
        }

        private void ListView_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            if( EnteredElementsListView.SelectedItem != null )
                _eventAggregator.GetEvent<OpenParsedElementsView>().Publish(
                    new ParseResponseDto
                    {
                        StructuredName = EnteredElementsListView.SelectedItem as StructuredName
                    }
                );
        }

        private void ListViewTitle_MouseDoubleClick( object sender, MouseButtonEventArgs e )
        {
            if( AvailableTitlesListView.SelectedItem != null && AvailableTitlesListView.SelectedItem as string != "Keine Titel verfügbar" )
                _eventAggregator.GetEvent<OpenRemoveTitleView>().Publish(AvailableTitlesListView.SelectedItem as string);
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