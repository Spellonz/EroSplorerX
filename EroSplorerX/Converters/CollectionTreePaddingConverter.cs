using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace EroSplorerX.Converters;

public class CollectionTreePaddingConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        if (value is string && (value as string).Contains('/'))
            return new Thickness(20, 0, 0, 0);

        return new Thickness(5, 0, 0, 0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
