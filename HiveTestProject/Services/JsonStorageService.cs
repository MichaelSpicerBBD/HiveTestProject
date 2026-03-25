using System.IO;
using System.Text.Json;
using HiveTestProject.Models;

namespace HiveTestProject.Services;

public class JsonStorageService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    private readonly string _storagePath;

    public JsonStorageService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appData, "HiveTestProject");
        Directory.CreateDirectory(appFolder);
        _storagePath = Path.Combine(appFolder, "notes.json");
    }

    public List<NoteItem> LoadNotes()
    {
        if (!File.Exists(_storagePath))
            return [];

        var json = File.ReadAllText(_storagePath);
        return JsonSerializer.Deserialize<List<NoteItem>>(json, JsonOptions) ?? [];
    }

    public void SaveNotes(IEnumerable<NoteItem> notes)
    {
        var json = JsonSerializer.Serialize(notes, JsonOptions);
        File.WriteAllText(_storagePath, json);
    }
}
