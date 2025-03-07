using Newtonsoft.Json;

namespace EroSplorerX.Data.AutoBlowApi.Responses;

public class GoToResponse
{
    [JsonProperty("success")]
    public bool Success { get; set; }
}
