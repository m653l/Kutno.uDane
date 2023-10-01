using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace AUI.Converters
{
    internal class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return ((DateTimeOffset)value).ToString();
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return new Avalonia.Data.BindingNotification(value);
        }
    }
}
