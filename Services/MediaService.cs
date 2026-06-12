using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using ShowsHub.Models;

namespace ShowsHub.Services;
public class MediaService
{
    private const string ApiKey = "a49b97ecacd18b487c8a519f8b9a37bb";
    private const string Base = "https://api.themoviedb.org/3";
    public const string ImgBase = "https://image.tmdb.org/t/p/w500";
    private static readonly HttpClient Http = new();

    public async Task<List<MediaItem>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return await GetPopularAsync();
        var url = $"{Base}/search/multi?api_key={ApiKey}&query={Uri.EscapeDataString(query)}&language=ro-RO";
        return await FetchItems(url);
    }

    public async Task<List<MediaItem>> GetPopularAsync()
    {
        var movies = FetchItems($"{Base}/movie/popular?api_key={ApiKey}&language=ro-RO&page=1");
        var shows = FetchItems($"{Base}/tv/popular?api_key={ApiKey}&language=ro-RO&page=1");
        var results = new List<MediaItem>();
        results.AddRange(await movies);
        results.AddRange(await shows);
        results.Sort((a, b) => b.Rating.CompareTo(a.Rating));
        return results;
    }

    public async Task<List<MediaItem>> GetTrendingAsync()
    {
        return await FetchItems($"{Base}/trending/all/week?api_key={ApiKey}&language=ro-RO");
    }

    public async Task<List<MediaItem>> GetTopRatedShowsAsync()
    {
        return await FetchItems($"{Base}/tv/top_rated?api_key={ApiKey}&language=ro-RO&page=1");
    }

    public async Task<List<MediaItem>> GetPopularMoviesAsync()
    {
        var page1 = FetchItems($"{Base}/movie/popular?api_key={ApiKey}&language=ro-RO&page=1");
        var page2 = FetchItems($"{Base}/movie/popular?api_key={ApiKey}&language=ro-RO&page=2");
        var results = new List<MediaItem>();
        results.AddRange(await page1);
        results.AddRange(await page2);
        results.Sort((a, b) => b.Rating.CompareTo(a.Rating));
        return results;
    }

    public async Task<List<MediaItem>> GetPopularSeriesAsync()
    {
        var page1 = FetchItems($"{Base}/tv/popular?api_key={ApiKey}&language=ro-RO&page=1");
        var page2 = FetchItems($"{Base}/tv/popular?api_key={ApiKey}&language=ro-RO&page=2");
        var results = new List<MediaItem>();
        results.AddRange(await page1);
        results.AddRange(await page2);
        results.Sort((a, b) => b.Rating.CompareTo(a.Rating));
        return results;
    }

    private async Task<List<MediaItem>> FetchItems(string url)
    {
        var list = new List<MediaItem>();
        try
        {
            var json = await Http.GetStringAsync(url);
            using var doc = JsonDocument.Parse(json);
            var results = doc.RootElement.GetProperty("results");
            foreach (var el in results.EnumerateArray())
            {
                var type = el.TryGetProperty("media_type", out var mt) ? mt.GetString() : null;
                var isMovie = type == "movie" || (el.TryGetProperty("title", out _) && !el.TryGetProperty("name", out _));
                var title = el.TryGetProperty("title", out var t) ? t.GetString()
                          : el.TryGetProperty("name", out var n) ? n.GetString() : "N/A";
                var posterPath = el.TryGetProperty("poster_path", out var p) ? p.GetString() : null;
                var poster = string.IsNullOrWhiteSpace(posterPath) ? "" : $"https://image.tmdb.org/t/p/w500{posterPath}";
                var rating = el.TryGetProperty("vote_average", out var r) ? Math.Round(r.GetDouble(), 1) : 0.0;
                var id = el.TryGetProperty("id", out var idEl) ? idEl.GetInt32() : 0;
                var overview = el.TryGetProperty("overview", out var ov) ? ov.GetString() ?? "" : "";
                var year = "";
                if (el.TryGetProperty("release_date", out var rd) && rd.GetString()?.Length >= 4)
                    year = rd.GetString()![..4];
                else if (el.TryGetProperty("first_air_date", out var fa) && fa.GetString()?.Length >= 4)
                    year = fa.GetString()![..4];

                if (type == "person" || string.IsNullOrWhiteSpace(title)) continue;

                list.Add(new MediaItem
                {
                    TmdbId = id,
                    Title = title!,
                    Type = isMovie ? "Film" : "Serial",
                    Poster = poster,
                    Rating = rating,
                    Overview = overview,
                    Year = year
                });
            }
        }
        catch { }
        return list;
    }
}
