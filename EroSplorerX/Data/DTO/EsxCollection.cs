using System.Text.Json.Serialization;

namespace EroSplorerX.Data.DTO;

public class EsxCollection
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public bool ShowChildren { get; set; } = false;

    [JsonIgnore]
    public string Tag { get; set; } = string.Empty;
}
