namespace EroSplorerX.Data.DTO;

public class EsxFile
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Length { get; set; } = string.Empty;
    public bool Played { get; set; }
    public string Performers { get; set; } = string.Empty;
}
