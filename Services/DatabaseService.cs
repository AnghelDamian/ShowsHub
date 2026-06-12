using Microsoft.Data.Sqlite;
using ShowsHub.Models;
using System.Collections.Generic;
using System.IO;

namespace ShowsHub.Services;
public class DatabaseService
{
    private readonly string _connStr;

    public DatabaseService()
    {
        var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ShowsHub");
        Directory.CreateDirectory(folder);
        var dbPath = Path.Combine(folder, "showshub.db");
        _connStr = $"Data Source={dbPath}";
        Init();
    }

    private void Init()
    {
        using var conn = new SqliteConnection(_connStr);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Favorites (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL,
                TmdbId INTEGER NOT NULL,
                Title TEXT NOT NULL,
                Type TEXT NOT NULL,
                Poster TEXT,
                Rating REAL,
                Overview TEXT,
                Year TEXT,
                UNIQUE(Username, TmdbId)
            );
            CREATE TABLE IF NOT EXISTS Watchlist (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL,
                TmdbId INTEGER NOT NULL,
                Title TEXT NOT NULL,
                Type TEXT NOT NULL,
                Poster TEXT,
                Rating REAL,
                Overview TEXT,
                Year TEXT,
                UNIQUE(Username, TmdbId)
            );";
        cmd.ExecuteNonQuery();
    }

    public void AddFavorite(string username, MediaItem item)
    {
        using var conn = new SqliteConnection(_connStr);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT OR IGNORE INTO Favorites (Username,TmdbId,Title,Type,Poster,Rating,Overview,Year)
                            VALUES ($u,$id,$t,$tp,$p,$r,$o,$y)";
        cmd.Parameters.AddWithValue("$u", username);
        cmd.Parameters.AddWithValue("$id", item.TmdbId);
        cmd.Parameters.AddWithValue("$t", item.Title);
        cmd.Parameters.AddWithValue("$tp", item.Type);
        cmd.Parameters.AddWithValue("$p", item.Poster);
        cmd.Parameters.AddWithValue("$r", item.Rating);
        cmd.Parameters.AddWithValue("$o", item.Overview);
        cmd.Parameters.AddWithValue("$y", item.Year);
        cmd.ExecuteNonQuery();
    }

    public void RemoveFavorite(string username, int tmdbId)
    {
        using var conn = new SqliteConnection(_connStr);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Favorites WHERE Username=$u AND TmdbId=$id";
        cmd.Parameters.AddWithValue("$u", username);
        cmd.Parameters.AddWithValue("$id", tmdbId);
        cmd.ExecuteNonQuery();
    }

    public List<MediaItem> GetFavorites(string username)
    {
        var list = new List<MediaItem>();
        using var conn = new SqliteConnection(_connStr);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT TmdbId,Title,Type,Poster,Rating,Overview,Year FROM Favorites WHERE Username=$u";
        cmd.Parameters.AddWithValue("$u", username);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add(new MediaItem { TmdbId = reader.GetInt32(0), Title = reader.GetString(1), Type = reader.GetString(2), Poster = reader.GetString(3), Rating = reader.GetDouble(4), Overview = reader.GetString(5), Year = reader.GetString(6) });
        return list;
    }

    public void AddWatchlist(string username, MediaItem item)
    {
        using var conn = new SqliteConnection(_connStr);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = @"INSERT OR IGNORE INTO Watchlist (Username,TmdbId,Title,Type,Poster,Rating,Overview,Year)
                            VALUES ($u,$id,$t,$tp,$p,$r,$o,$y)";
        cmd.Parameters.AddWithValue("$u", username);
        cmd.Parameters.AddWithValue("$id", item.TmdbId);
        cmd.Parameters.AddWithValue("$t", item.Title);
        cmd.Parameters.AddWithValue("$tp", item.Type);
        cmd.Parameters.AddWithValue("$p", item.Poster);
        cmd.Parameters.AddWithValue("$r", item.Rating);
        cmd.Parameters.AddWithValue("$o", item.Overview);
        cmd.Parameters.AddWithValue("$y", item.Year);
        cmd.ExecuteNonQuery();
    }

    public void RemoveWatchlist(string username, int tmdbId)
    {
        using var conn = new SqliteConnection(_connStr);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "DELETE FROM Watchlist WHERE Username=$u AND TmdbId=$id";
        cmd.Parameters.AddWithValue("$u", username);
        cmd.Parameters.AddWithValue("$id", tmdbId);
        cmd.ExecuteNonQuery();
    }

    public List<MediaItem> GetWatchlist(string username)
    {
        var list = new List<MediaItem>();
        using var conn = new SqliteConnection(_connStr);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT TmdbId,Title,Type,Poster,Rating,Overview,Year FROM Watchlist WHERE Username=$u";
        cmd.Parameters.AddWithValue("$u", username);
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add(new MediaItem { TmdbId = reader.GetInt32(0), Title = reader.GetString(1), Type = reader.GetString(2), Poster = reader.GetString(3), Rating = reader.GetDouble(4), Overview = reader.GetString(5), Year = reader.GetString(6) });
        return list;
    }

    public bool IsFavorite(string username, int tmdbId)
    {
        using var conn = new SqliteConnection(_connStr);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM Favorites WHERE Username=$u AND TmdbId=$id";
        cmd.Parameters.AddWithValue("$u", username);
        cmd.Parameters.AddWithValue("$id", tmdbId);
        return (long)cmd.ExecuteScalar()! > 0;
    }

    public bool IsWatchlist(string username, int tmdbId)
    {
        using var conn = new SqliteConnection(_connStr);
        conn.Open();
        var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM Watchlist WHERE Username=$u AND TmdbId=$id";
        cmd.Parameters.AddWithValue("$u", username);
        cmd.Parameters.AddWithValue("$id", tmdbId);
        return (long)cmd.ExecuteScalar()! > 0;
    }
}
