using Plugin.LocalNotification;
using System.Globalization;

namespace MAUI4Maui.Converters;

public class RepeatTypeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is NotificationRepeat nr)
            return nr == NotificationRepeat.TimeInterval;

        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        value is true ? NotificationRepeat.TimeInterval : NotificationRepeat.No;
}