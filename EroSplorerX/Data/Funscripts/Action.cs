using Newtonsoft.Json;

namespace EroSplorerX.Data.Funscripts;

public class Action
{
    [JsonProperty("pos")]
    public int Pos { get; set; }

    [JsonProperty("at")]
    public int At { get; set; }
}

