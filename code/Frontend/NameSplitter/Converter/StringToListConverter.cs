using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace NameSplitter.Converter
{
    /// <summary>
    /// Converts IEnumerable<string> to joined string and backwards
    /// </summary>
    public class StringToListConverter: IValueConverter
    {
        /// <summary>
        /// Converts a IEnumerable<string> to a string, joined with ", "
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>string as object</returns>
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( value is IEnumerable<string> stringList )
                return string.Join(", ", stringList);

            return "";
        }

        /// <summary>
        /// Splits a string at "," and converts it to a list of strings
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns>List<string> as object</returns>
        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if( value is string stringValue )
                return stringValue.Split(',').Where(element => element is not "" && element is not " ").ToList();

            return new List<string>();
        }
    }
}