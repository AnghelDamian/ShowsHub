using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Collections.Concurrent;

namespace ShowsHub.Converters;

public class PosterConverter : IValueConverter
{
    public static readonly PosterConverter Instance = new();
    private static readonly HttpClient Http = new();
    private static readonly ConcurrentDictionary<string, Bitmap?> Cache = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string url || string.IsNullOrWhiteSpace(url))
            return null;

        if (Cache.TryGetValue(url, out var cached))
            return cached;

        try
        {
            var bytes = Http.GetByteArrayAsync(url).Result;
            using var ms = new MemoryStream(bytes);
            var bmp = new Bitmap(ms);
            Cache[url] = bmp;
            return bmp;
        }
        catch
        {
            Cache[url] = null;
            return null;
        }
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}