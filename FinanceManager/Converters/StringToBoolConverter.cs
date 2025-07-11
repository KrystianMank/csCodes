﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FinanceManager.Converters
{
    public class StringToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Przychód" : "Wydatek";
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (string)value == parameter?.ToString() ? true : false;
        }
    }
}
