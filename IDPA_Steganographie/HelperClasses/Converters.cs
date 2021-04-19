using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace IDPA_Steganographie.HelperClasses
{
    public static class Converters
    {
        private static IValueConverter _trueToGreenConverter = new TrueToGreenConverter();

        public static IValueConverter TrueToGreenConverter
        {
            get
            {
                return _trueToGreenConverter;
            }
        }

        public static IValueConverter TrueToVisbleConverter
        {
            get
            {
                return new BooleanToVisibilityConverter();
            }
        }
    }

    public class TrueToGreenConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                bool isValid = (bool)value;
                return isValid ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
            }
            throw new ArgumentException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
