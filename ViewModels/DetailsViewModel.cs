using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShowsHub.Services;

namespace ShowsHub.ViewModels;
public partial class DetailsViewModel : ObservableObject
{
    private readonly MainWindowViewModel _nav;
    private readonly DatabaseService _db = new();
    private readonly string _username;
    public Models.MediaItem Item { get; }

    [ObservableProperty] private bool isFavorite;
    [ObservableProperty] private bool isWatchlist;

    public DetailsViewModel(MainWindowViewModel nav, Models.MediaItem item, string username)
    {
        _nav = nav;
        Item = item;
        _username = username;
        IsFavorite = _db.IsFavorite(_username, item.TmdbId);
        IsWatchlist = _db.IsWatchlist(_username, item.TmdbId);
    }

    [RelayCommand]
    private void Back() => _nav.ShowHome(_username);

    [RelayCommand]
    private void ToggleFavorite()
    {
        if (IsFavorite) { _db.RemoveFavorite(_username, Item.TmdbId); IsFavorite = false; }
        else { _db.AddFavorite(_username, Item); IsFavorite = true; }
    }

    [RelayCommand]
    private void ToggleWatchlist()
    {
        if (IsWatchlist) { _db.RemoveWatchlist(_username, Item.TmdbId); IsWatchlist = false; }
        else { _db.AddWatchlist(_username, Item); IsWatchlist = true; }
    }
}
