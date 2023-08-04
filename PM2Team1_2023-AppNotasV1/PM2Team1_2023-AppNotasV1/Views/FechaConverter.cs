using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PM2Team1_2023_AppNotasV1.Views
{
public class FechaConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy");
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
}
