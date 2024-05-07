using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace NameSplitter.DTOs
{
    public class TextWithColor
    {
        public TextWithColor( string text, SolidColorBrush color )
        {
            Text = text;
            Color = color;
        }

        public SolidColorBrush Color { get; set; }
        public string Text { get; set; }
    }
}