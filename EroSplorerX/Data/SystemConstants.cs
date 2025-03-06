using Microsoft.UI.Xaml;
using System.IO;
using System.Reflection;

namespace EroSplorerX.Data;

internal static class SystemConstants
{
    public static readonly string APP_PATH = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    public static readonly string APP_DATA_PATH = Path.Combine(APP_PATH, "Data");
    public static readonly string COLLECTIONS_PATH = Path.Combine(APP_DATA_PATH, "collections.json");
    public static readonly string TIMES_PATH = Path.Combine(APP_DATA_PATH, "times.dat");

    public static readonly string DEFAULT_THUMBNAIL_PATH = Path.Combine(APP_DATA_PATH, "NoPreview.gif");
    public static readonly string DEFAULT_HEATMAP_PATH = Path.Combine(APP_DATA_PATH, "NoHeatmap.png");

    public const string THUMBNAIL_EXTENSION= ".gif";
    public const string HEATMAP_EXTENSION = ".png";
    public const string FUNSCRIPT_EXTENSION = ".funscript";

    public const int INFOBAR_DURATION_DEFAULT = 10000;
    public const int INFOBAR_DURATION_SHORT = 5000;
    public const int INFOBAR_DURATION_VERY_SHORT = 2000;

    public const string FFMPEG_PATH = @"C:\ffmpeg-2024-03-07-git-97beb63a66-essentials_build\bin";
    public const string TEMP_FILE_PATH = @"C:\Temp";

    public const string AUTOBLOW_DEVICE_ID = "yjij3kbke3wf";
}