using NameSplitter.DTOs;
using NameSplitter.Events;
using Prism.Events;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace NameSplitter.Views
{
    /// <summary>
    /// Interaction logic for ParsedElements.xaml
    /// </summary>
    public partial class ParsedElementsView : Window
    {
        private IEventAggregator _eventAggregator;

        public ParsedElementsView( IEventAggregator eventAggregator )
        {
            _eventAggregator = eventAggregator;
            InitializeComponent();
        }

        public void WriteInTextbox( List<TextWithColor> words )
        {
            foreach (var word in words)
            {
                MyTextBlock.Inlines.Add(new Run
                {
                    Text = word.Text,
                    Foreground = word.Color
                });
            }
        }
    }
}