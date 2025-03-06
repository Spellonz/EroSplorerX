using Newtonsoft.Json;

namespace EroSplorerX.Data.AutoBlowApi.Responses;

public class ConnectedResponse
{
    /// <summary>
    /// True if the device is currently connected
    /// </summary>
    [JsonProperty("connected")]
    public bool Connected { get; set; }

    /// <summary>
    /// URI to the connected cluster, undefined if not connected
    /// </summary>
    [JsonProperty("cluster")]
    public string Cluster { get; set; } = string.Empty;

}
