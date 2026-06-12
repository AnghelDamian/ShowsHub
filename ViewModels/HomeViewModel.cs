using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShowsHub.Services;
using System.Collections.ObjectModel;
using System.Linq;

namespace ShowsHub.ViewModels;
public partial class HomeViewModel : ObservableObject
{
    private readonly MainWindowViewModel _nav;
    private readonly MediaService _mediaService = new();
    private readonly DatabaseService _db = new();
    private readonly string _username;

    public ObservableCollection<Models.MediaItem> Items { get; } = new();

    [ObservableProperty] private string searchText = string.Empty;
    [ObservableProperty] private bool isLoading = false;
    [ObservableProperty] private string sectionTitle = "🔥 Trending";
    [ObservableProperty] private string itemCountText = string.Empty;
    [ObservableProperty] private bool isEmpty = false;
    [ObservableProperty] private string emptyMessage = "🎬";
    [ObservableProperty] private string emptySubMessage = string.Empty;
    [ObservableProperty] private string welcomeText = string.Empty;

    public HomeViewModel(MainWindowViewModel nav, string username)
    {
        _nav = nav;
        _username = username;
        WelcomeText = $"Salut, {username}";
        _ = ShowTrendingAsync();
    }

    private async Task LoadItemsAsync(System.Func<Task<System.Collections.Generic.List<Models.MediaItem>>> loader, string title, string emptyMsg)
    {
        IsLoading = true;
        IsEmpty = false;
        SectionTitle = title;
        Items.Clear();
        var results = await loader();
        foreach (var item in results)
        {
            if (!item.Poster.StartsWith("http"))
                item.Poster = string.IsNullOrWhiteSpace(item.Poster)
                    ? string.Empty
                    : $"https://image.tmdb.org/t/p/w500{item.Poster}";
            Items.Add(item);
        }
        IsLoading = false;
        ItemCountText = Items.Count > 0 ? $"({Items.Count} rezultate)" : string.Empty;
        if (Items.Count == 0)
        {
            IsEmpty = true;
            EmptyMessage = "🎬";
            EmptySubMessage = emptyMsg;
        }
    }

    private void LoadListItems(System.Collections.Generic.List<Models.MediaItem> list, string title, string emptyMsg)
    {
        IsLoading = false;
        IsEmpty = false;
        SectionTitle = title;
        Items.Clear();
        foreach (var item in list) Items.Add(item);
        ItemCountText = Items.Count > 0 ? $"({Items.Count} rezultate)" : string.Empty;
        if (Items.Count == 0)
        {
            IsEmpty = true;
            EmptyMessage = "💔";
            EmptySubMessage = emptyMsg;
        }
    }

    [RelayCommand]
    private async Task ShowTrending() => await ShowTrendingAsync();

    private async Task ShowTrendingAsync() =>
        await LoadItemsAsync(() => _mediaService.GetTrendingAsync(), "🔥 Trending", "Niciun rezultat trending disponibil.");

    [RelayCommand]
    private async Task ShowPopular() =>
        await LoadItemsAsync(() => _mediaService.GetPopularAsync(), "🎬 Populare", "Niciun rezultat disponibil.");

    [RelayCommand]
    private async Task ShowMovies() =>
        await LoadItemsAsync(() => _mediaService.GetPopularMoviesAsync(), "🎭 Filme", "Niciun film disponibil.");

    [RelayCommand]
    private async Task ShowSeries() =>
        await LoadItemsAsync(() => _mediaService.GetPopularSeriesAsync(), "📺 Seriale", "Niciun serial disponibil.");

    [RelayCommand]
    private void ShowFavorites() =>
        LoadListItems(_db.GetFavorites(_username), "♥ Favorite", "Nu ai adăugat niciun favorit încă.\nApasă Detalii pe un film și adaugă-l!");

    [RelayCommand]
    private void ShowWatchlist() =>
        LoadListItems(_db.GetWatchlist(_username), "🕐 Watchlist", "Watchlist-ul tău e gol.\nAdaugă filme din pagina de Detalii!");

    [RelayCommand]
    private async Task Search()
    {
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            await ShowTrendingAsync();
            return;
        }
        await LoadItemsAsync(() => _mediaService.SearchAsync(SearchText), $"🔍 Rezultate pentru \"{SearchText}\"", $"Niciun rezultat pentru \"{SearchText}\".");
    }

    [RelayCommand]
    private void OpenDetails(Models.MediaItem item) => _nav.ShowDetails(item, _username);

    [RelayCommand]
    private void NavigateSettings() => _nav.ShowSettings(_username);
}
