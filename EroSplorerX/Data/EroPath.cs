using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EroSplorerX.Data;

public class EroPath
{
    public string Name { get; set; } = string.Empty;
    public string FullPath { get; set; } = string.Empty;

    public string[] Files { get; set; } = [];
    public List<EroPath> Directories { get; set; } = [];

    public bool HasFunscript => !string.IsNullOrEmpty(FunScriptPath);
    public string FunScriptPath => Files.FirstOrDefault(x => x.EndsWith(SystemConstants.FUNSCRIPT_EXTENSION)) ?? string.Empty;
    
    public bool HasThumbnail => !string.IsNullOrEmpty(ThumbnailPath);
    public string ThumbnailPath => Files.FirstOrDefault(x => x.EndsWith(SystemConstants.THUMBNAIL_EXTENSION)) ?? string.Empty;
    
    public bool HasHeatmap => !string.IsNullOrEmpty(HeatmapPath);
    public string HeatmapPath => Files.FirstOrDefault(x => x.EndsWith(SystemConstants.HEATMAP_EXTENSION)) ?? string.Empty;

    public EroPath(string path)
    {
        Name = Path.GetFileName(path);
        FullPath = path;

        Files = Directory.GetFiles(path);
        Directories = Directory.GetDirectories(FullPath).Select(x => new EroPath(x)).ToList();
    }

    public List<EroPath> GetFunscripts()
    {
        var ret = new List<EroPath>();
        if (HasFunscript)
            ret.Add(this);

        foreach (var directory in Directories)
            ret.AddRange(directory.GetFunscripts());

        return ret;
    }



}
