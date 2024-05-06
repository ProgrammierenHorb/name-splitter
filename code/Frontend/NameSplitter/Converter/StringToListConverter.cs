using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace NameSplitter.Converter
{
    public class StringToListConverter: IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( value is IEnumerable<string> stringList )
            {
                return string.Join(", ", stringList);
            }
            return "";
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( value is string stringValue )
                return stringValue.Split(',').Where(element => element is not "" && element is not " ").ToList();

            return new List<string>();
        }
    }
}