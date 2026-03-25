using System.Collections.ObjectModel;
using System.Windows.Input;
using HiveTestProject.Models;
using HiveTestProject.Services;

namespace HiveTestProject.ViewModels;

public class NotesViewModel : BaseViewModel
{
    private readonly JsonStorageService _storage;
    private NoteItem? _selectedNote;
    private string _editorContent = string.Empty;

    public ObservableCollection<NoteItem> Notes { get; } = [];

    public NoteItem? SelectedNote
    {
        get => _selectedNote;
        set
        {
            if (SetProperty(ref _selectedNote, value))
            {
                EditorContent = value?.Content ?? string.Empty;
            }
        }
    }

    public string EditorContent
    {
        get => _editorContent;
        set
        {
            if (SetProperty(ref _editorContent, value) && _selectedNote is not null)
            {
                _selectedNote.Content = value;
                _selectedNote.ModifiedAt = DateTime.Now;
                Save();
            }
        }
    }

    public ICommand AddNoteCommand { get; }
    public ICommand DeleteNoteCommand { get; }

    public NotesViewModel(JsonStorageService storage)
    {
        _storage = storage;
        AddNoteCommand = new RelayCommand(AddNote);
        DeleteNoteCommand = new RelayCommand(DeleteNote, () => SelectedNote is not null);

        foreach (var note in _storage.LoadNotes())
            Notes.Add(note);

        if (Notes.Count > 0)
            SelectedNote = Notes[0];
    }

    private void AddNote()
    {
        var note = new NoteItem { Content = "New note" };
        Notes.Add(note);
        SelectedNote = note;
        Save();
    }

    private void DeleteNote()
    {
        if (SelectedNote is null) return;

        var index = Notes.IndexOf(SelectedNote);
        Notes.Remove(SelectedNote);

        if (Notes.Count > 0)
            SelectedNote = Notes[Math.Min(index, Notes.Count - 1)];
        else
            SelectedNote = null;

        Save();
    }

    private void Save() => _storage.SaveNotes(Notes);
}
