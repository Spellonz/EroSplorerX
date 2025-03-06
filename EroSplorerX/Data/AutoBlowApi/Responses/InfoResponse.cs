using Newtonsoft.Json;

namespace EroSplorerX.Data.AutoBlowApi.Responses;

public class InfoResponse
{
    /// <summary>
    /// The firmware status
    /// </summary>
    [JsonProperty("firmwareStatus")]
    public string FirmwareStatus { get; set; } = string.Empty;

    /// <summary>
    /// The current device firmware version
    /// </summary>
    [JsonProperty("firmwareVersion")]
    public float FirmwareVersion { get; set; }

    /// <summary>
    /// The firmware branch
    /// </summary>
    [JsonProperty("firmwareBranch")]
    public string FirmwareBranch { get; set; } = string.Empty;

    /// <summary>
    /// The hardware version
    /// </summary>
    [JsonProperty("hardwareVersion")]
    public string HardwareVersion { get; set; } = string.Empty;

    /// <summary>
    /// The device mac address
    /// </summary>
    [JsonProperty("mac")]
    public string Mac { get; set; } = string.Empty;
}
