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
    private readonly string _todosPath;

    public JsonStorageService()
    {
        var appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var appFolder = Path.Combine(appData, "HiveTestProject");
        Directory.CreateDirectory(appFolder);
        _storagePath = Path.Combine(appFolder, "notes.json");
        _todosPath = Path.Combine(appFolder, "todos.json");
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

    public List<TodoItem> LoadTodos()
    {
        if (!File.Exists(_todosPath))
            return [];

        var json = File.ReadAllText(_todosPath);
        return JsonSerializer.Deserialize<List<TodoItem>>(json, JsonOptions) ?? [];
    }

    public void SaveTodos(IEnumerable<TodoItem> todos)
    {
        var json = JsonSerializer.Serialize(todos, JsonOptions);
        File.WriteAllText(_todosPath, json);
    }
}
