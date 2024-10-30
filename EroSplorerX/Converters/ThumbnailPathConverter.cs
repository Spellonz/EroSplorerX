using EroSplorerX.Data;
using Microsoft.UI.Xaml.Data;
using System;
using System.IO;

namespace EroSplorerX.Converters;

public class ThumbnailPathConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        var path = value as string;

        if (!string.IsNullOrEmpty(path) && File.Exists(path))
            return path; // Return the original path if it exists

        // Return a default path
        return SystemConstants.DEFAULT_THUMBNAIL_PATH;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
