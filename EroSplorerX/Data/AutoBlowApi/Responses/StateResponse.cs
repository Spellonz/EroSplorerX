using Newtonsoft.Json;

namespace EroSplorerX.Data.AutoBlowApi.Responses;

public class StateResponse
{
    /// <summary>
    /// The current operational mode of the device
    /// </summary>
    [JsonProperty("operationalMode")]
    public string OperationalMode { get; set; } = string.Empty;

    /// <summary>
    /// The index of the local script
    /// </summary>
    [JsonProperty("localScript")]
    public int LocalScript { get; set; }

    /// <summary>
    /// The speed index of the local script
    /// </summary>
    [JsonProperty("localScriptSpeed")]
    public int LocalScriptSpeed { get; set; }

    /// <summary>
    /// The motor temperature in °C
    /// </summary>
    [JsonProperty("motorTemperature")]
    public int MotorTemperature { get; set; }

    /// <summary>
    /// The oscillator target speed in percent
    /// </summary>
    [JsonProperty("oscillatorTargetSpeed")]
    public int OscillatorTargetSpeed { get; set; }

    /// <summary>
    /// The oscillator low point in percent
    /// </summary>
    [JsonProperty("oscillatorLowPoint")]
    public int OscillatorLowPoint { get; set; }

    /// <summary>
    /// The oscillator high point in percent
    /// </summary>
    [JsonProperty("oscillatorHighPoint")]
    public int OscillatorHighPoint { get; set; }

    /// <summary>
    /// The current time of the sync script in MS
    /// </summary>
    [JsonProperty("syncScriptCurrentTime")]
    public int SyncScriptCurrentTime { get; set; }

    /// <summary>
    /// The offset time of the sync script in MS
    /// </summary>
    [JsonProperty("syncScriptOffsetTime")]
    public int SyncScriptOffsetTime { get; set; }

    /// <summary>
    /// The loaded sync script token
    /// </summary>
    [JsonProperty("syncScriptToken")]
    public string SyncScriptToken { get; set; } = string.Empty;

    /// <summary>
    /// If set to true, it will restart sync script when finished
    /// </summary>
    [JsonProperty("syncScriptLoop")]
    public bool SyncScriptLoop { get; set; }
}
