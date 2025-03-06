using Newtonsoft.Json;

namespace EroSplorerX.Data.Funscripts;

public class SimulatorPreset
{
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [JsonProperty("fullRange")]
    public bool FullRange { get; set; }

    [JsonProperty("direction")]
    public int Direction { get; set; }

    [JsonProperty("rotation")]
    public double Rotation { get; set; }

    [JsonProperty("length")]
    public double Length { get; set; }

    [JsonProperty("width")]
    public double Width { get; set; }

    [JsonProperty("offset")]
    public string Offset { get; set; } = string.Empty;

    [JsonProperty("color")]
    public string Color { get; set; } = string.Empty;
}

