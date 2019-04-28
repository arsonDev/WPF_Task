﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ZadaniaWPF.Model;

namespace ZadaniaWPF.Converters
{
    class PriorityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((Priority)value)
            {
                case Priority.MniejWażne:
                    return "Mniej ważne";
                case Priority.Ważne:
                    return "Ważne";
                case Priority.Krytyczne:
                    return "Krytyczne";
                default:
                    throw new Exception("Brak nadanego priorytetu");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            switch ((string)value)
            {
                case "Mniej ważne":
                    return Priority.MniejWażne;
                case "Ważne":
                    return Priority.Ważne;
                case "Krytyczne":
                    return Priority.Krytyczne;
                default:
                    throw new Exception("Brak nadanego priorytetu");
            }
        }
    }
}
