using Microsoft.EntityFrameworkCore;
using ShowsHub.Models;

namespace ShowsHub.Data;
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<FavoriteItem> Favorites { get; set; }
    public DbSet<WatchlistItem> Watchlist { get; set; }
    public string DbPath { get; }

    public AppDbContext()
    {
        var folder = System.IO.Path.Combine(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData),
            "ShowsHub");
        System.IO.Directory.CreateDirectory(folder);
        DbPath = System.IO.Path.Combine(folder, "shows.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}