namespace ShowsHub.Models;
public class MediaItem
{
    public int TmdbId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string Poster { get; set; } = string.Empty;
    public string Overview { get; set; } = string.Empty;
    public string Year { get; set; } = string.Empty;
    public bool HasPoster => !string.IsNullOrWhiteSpace(Poster);
}
