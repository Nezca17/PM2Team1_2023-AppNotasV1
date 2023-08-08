using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace PM2Team1_2023_AppNotasV1.Converters
{
public class StringToUriConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string urlString)
        {
            Uri uriResult;
            if (Uri.TryCreate(urlString, UriKind.Absolute, out uriResult))
            {
                return uriResult;
            }
        }
        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
}
