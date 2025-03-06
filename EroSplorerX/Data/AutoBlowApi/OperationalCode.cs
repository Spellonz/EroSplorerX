namespace EroSplorerX.Data.AutoBlowApi;

public enum OperationalCode
{
    /// <summary>
    /// Device is connected to the server and ready to receive commands
    /// </summary>
    ONLINE_CONNECTED,

    /// <summary>
    /// The device is playing a sync script
    /// syncScriptToken, syncScriptCurrentTime, syncScriptOffsetTime, syncScriptLoop
    /// </summary>
    SYNC_SCRIPT_PLAYING,

    /// <summary>
    /// Sync script is paused
    /// </summary>
    SYNC_SCRIPT_PAUSED,

    /// <summary>
    /// Local script is playing
    /// localScript,localScriptSpeed
    /// </summary>
    LOCAL_SCRIPT_PLAYING,

    /// <summary>
    /// Local script is paused
    /// </summary>
    LOCAL_SCRIPT_PAUSED,

    /// <summary>
    /// Oscillator is playing
    /// oscillatorTargetSpeed, oscillatorLowPoint, oscillatorHighPoint
    /// </summary>
    OSCILLATOR_PLAYING,

    /// <summary>
    /// Oscillator is paused
    /// </summary>
    OSCILLATOR_PAUSED,

    /// <summary>
    /// Device is moving to a specific position
    /// </summary>
    GO_TO,

    /// <summary>
    /// Device is updating its firmware
    /// </summary>
    FIRMWARE_UPDATING,

    /// <summary>
    /// Device is in an error state because the motor is stuck
    /// </summary>
    ERROR_MOTOR_STUCK,

    /// <summary>
    /// Device is in an error state because the device is overheating
    /// </summary>
    ERROR_OVERHEATING,

    /// <summary>
    /// Device is in an error state
    /// </summary>
    ERROR
}
