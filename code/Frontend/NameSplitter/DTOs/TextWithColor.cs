using System.Windows.Media;

namespace NameSplitter.DTOs
{
    /// <summary>
    /// Contains the properties to colour a specific text.
    /// Used for error messages
    /// </summary>
    public class TextWithColor
    {
        public SolidColorBrush Color { get; set; }

        public string Text { get; set; }

        public TextWithColor( string text, SolidColorBrush color )
        {
            Text = text;
            Color = color;
        }
    }
}