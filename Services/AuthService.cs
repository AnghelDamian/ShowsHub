using ShowsHub.Data;
using ShowsHub.Models;
using System.Linq;

namespace ShowsHub.Services;
public class AuthService
{
    public bool Register(string username, string password)
    {
        try
        {
            using var db = new AppDbContext();
            db.Database.EnsureCreated();
            if (db.Users.Any(u => u.Username == username)) return false;
            var u = new User { Username = username, PasswordHash = password };
            db.Users.Add(u);
            db.SaveChanges();
            return true;
        }
        catch (System.Exception ex)
        {
            LogError(ex);
            return false;
        }
    }
    public bool Login(string username, string password)
    {
        try
        {
            using var db = new AppDbContext();
            db.Database.EnsureCreated();
            return db.Users.Any(u => u.Username == username && u.PasswordHash == password);
        }
        catch (System.Exception ex)
        {
            LogError(ex);
            return false;
        }
    }

    private void LogError(System.Exception ex)
    {
        try
        {
            var folder = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData), "ShowsHub");
            System.IO.Directory.CreateDirectory(folder);
            var path = System.IO.Path.Combine(folder, "error.log");
            System.IO.File.AppendAllText(path, $"[{System.DateTime.Now}] {ex}\r\n");
        }
        catch { }
    }
}
