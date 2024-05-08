using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace NameSplitter.Validation
{
    public class TextBoxValidation: ValidationRule
    {
        /// <summary>
        /// Validation of input value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate( object value, CultureInfo cultureInfo )
        {
            if( string.IsNullOrEmpty(value as string) )
                return new ValidationResult(false, "Bitte geben Sie einen Wert ein.");

            if( Regex.IsMatch(value as string, @"\d+") )
                return new ValidationResult(false, "Bitte verwenden Sie keine Zahlen.");

            return ValidationResult.ValidResult;
        }
    }
}