﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace NameSplitter.Validation
{
    public class TextBoxInputValidation : ValidationRule
    {
        public TextBoxInputValidation()
        {
        }


        public override ValidationResult Validate( object value, CultureInfo cultureInfo )
        {
            if (string.IsNullOrEmpty(value as string))
                return new ValidationResult(false, "Bitte geben Sie einen Wert ein.");

            if (Regex.IsMatch(value as string, @"\d+"))
                return new ValidationResult(false, "Bitte verwenden Sie keine Zahlen.");

            return ValidationResult.ValidResult;
        }
    }
}