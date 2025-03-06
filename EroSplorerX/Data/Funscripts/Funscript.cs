using Newtonsoft.Json;
using System.Collections.Generic;

namespace EroSplorerX.Data.Funscripts;

public class Funscript
{
    [JsonProperty("version")]
    public string Version { get; set; } = string.Empty;

    [JsonProperty("inverted")]
    public bool Inverted { get; set; }

    [JsonProperty("range")]
    public int Range { get; set; }

    [JsonProperty("bookmark")]
    public int Bookmark { get; set; }

    [JsonProperty("lastPosition")]
    public long LastPosition { get; set; }

    [JsonProperty("graphDuration")]
    public int GraphDuration { get; set; }

    [JsonProperty("speedRatio")]
    public double SpeedRatio { get; set; }

    [JsonProperty("injectionSpeed")]
    public int InjectionSpeed { get; set; }

    [JsonProperty("injectionBias")]
    public double InjectionBias { get; set; }

    [JsonProperty("scriptingMode")]
    public int ScriptingMode { get; set; }

    [JsonProperty("simulatorPresets")]
    public List<SimulatorPreset> SimulatorPresets { get; set; } = [];

    [JsonProperty("activeSimulator")]
    public int ActiveSimulator { get; set; }

    [JsonProperty("reductionTolerance")]
    public double ReductionTolerance { get; set; }

    [JsonProperty("reductionStretch")]
    public double ReductionStretch { get; set; }

    [JsonProperty("clips")]
    public List<object> Clips { get; set; } = [];

    [JsonProperty("actions")]
    public List<Action> Actions { get; set; } = [];

    [JsonProperty("rawActions")]
    public List<RawAction> RawActions { get; set; } = [];
}