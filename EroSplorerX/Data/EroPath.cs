using EroSplorerX.Helpers;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EroSplorerX.Data;

public class EroPath
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FullPath { get; set; } = string.Empty;
    public bool Played { get; set; }

    public string[] Files { get; set; } = [];
    public List<EroPath> Directories { get; set; } = [];


    public bool HasVideo => !string.IsNullOrEmpty(VideoPath);
    public string VideoPath => Files.FirstOrDefault(x => x.EndsWith(".mp4") || x.EndsWith(".avi") || x.EndsWith(".mkv")) ?? string.Empty;
    public bool HasFunscript => !string.IsNullOrEmpty(FunScriptPath);
    public string FunScriptPath => Files.FirstOrDefault(x => x.EndsWith(SystemConstants.FUNSCRIPT_EXTENSION)) ?? string.Empty;
    
    public bool HasThumbnail => !string.IsNullOrEmpty(ThumbnailPath);
    public string ThumbnailPath => Files.FirstOrDefault(x => x.EndsWith(SystemConstants.THUMBNAIL_EXTENSION)) ?? string.Empty;
    
    public bool HasHeatmap => !string.IsNullOrEmpty(HeatmapPath);
    public string HeatmapPath => Files.FirstOrDefault(x => x.EndsWith(SystemConstants.HEATMAP_EXTENSION)) ?? string.Empty;

    public string VideoLength => GetVideoLength();

    public EroPath(string path)
    {
        Name = Path.GetFileName(path);
        FullPath = path;

        Files = Directory.GetFiles(path);
        Directories = Directory.GetDirectories(FullPath).Select(x => new EroPath(x)).ToList();

        if (HasFunscript)
        {
            var metadata = DatabaseHelper.GetMetadataForPath(FullPath);
            if (metadata == null)
            {
                Id = DatabaseHelper.InsertFile(Name, FullPath);
                return;
            }

            Id = metadata.Id;
            Played = metadata.Played;
        }
    }

    private string GetVideoLength()
    {
        if (!HasVideo || Id == 0) return string.Empty;

        TimeSpan time;

        var cachedTime = DatabaseHelper.GetTimeForId(Id);
        if (!string.IsNullOrEmpty(cachedTime))
        {
            time = TimeSpan.Parse(cachedTime);
        }
        else
        {
            using var reader = new MediaFoundationReader(VideoPath);
            time = reader.TotalTime;
            DatabaseHelper.InsertTimeForId(Id, time.ToString());
        }

        if (time.TotalSeconds < 60)
            return time.ToString(@"s") + "s";
        else
        if (time.TotalSeconds < 3600)
            return time.ToString(@"m\:ss");
        else
            return time.ToString(@"h\:mm\:ss");
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
