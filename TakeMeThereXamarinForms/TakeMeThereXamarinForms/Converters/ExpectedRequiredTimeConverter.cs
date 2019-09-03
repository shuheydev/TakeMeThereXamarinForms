using System;
using System.Globalization;
using Xamarin.Forms;

namespace TakeMeThereXamarinForms.Converters
{
    public class ExpectedRequiredTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var timeSpan = (TimeSpan)value;

            return $"{timeSpan.Hours.ToString("F0")}H {timeSpan.Minutes.ToString("F0")}M {timeSpan.Seconds.ToString("F0")}S";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}