namespace ShowsHub.Models;

public class FavoriteItem
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public int TmdbId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string Poster { get; set; } = string.Empty;
}

public class WatchlistItem
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public int TmdbId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public double Rating { get; set; }
    public string Poster { get; set; } = string.Empty;
}