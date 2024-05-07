using NameSplitter.Events;
using Prism.Events;
using System.Collections.Generic;
using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using NameSplitter.DTOs;

namespace NameSplitter.Views
{
    /// <summary>
    /// Interaction logic for ParsedElements.xaml
    /// </summary>
    public partial class ParsedElements : Window
    {
        private IEventAggregator _eventAggregator;

        private void Window_KeyUp( object sender, KeyEventArgs e )
        {
            if (e.Key == Key.Enter)
                _eventAggregator.GetEvent<SaveParsedElements>().Publish();
        }

        public ParsedElements( IEventAggregator eventAggregator )
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