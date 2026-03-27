using System.Collections.ObjectModel;
using System.Windows.Input;
using HiveTestProject.Models;
using HiveTestProject.Services;

namespace HiveTestProject.ViewModels;

public class TodoViewModel : BaseViewModel
{
    private readonly JsonStorageService _storage;
    private string _newItemTitle = string.Empty;
    private int _completedCount;
    private int _totalCount;

    public ObservableCollection<TodoItem> Todos { get; } = [];

    public string NewItemTitle
    {
        get => _newItemTitle;
        set => SetProperty(ref _newItemTitle, value);
    }

    public int CompletedCount
    {
        get => _completedCount;
        private set => SetProperty(ref _completedCount, value);
    }

    public int TotalCount
    {
        get => _totalCount;
        private set => SetProperty(ref _totalCount, value);
    }

    public ICommand AddTodoCommand { get; }
    public ICommand DeleteTodoCommand { get; }
    public ICommand ToggleCompletedCommand { get; }
    public ICommand ClearCompletedCommand { get; }

    public TodoViewModel(JsonStorageService storage)
    {
        _storage = storage;

        AddTodoCommand = new RelayCommand(AddTodo, () => !string.IsNullOrWhiteSpace(NewItemTitle));
        DeleteTodoCommand = new RelayCommand(p => DeleteTodo(p as TodoItem));
        ToggleCompletedCommand = new RelayCommand(p => ToggleCompleted(p as TodoItem));
        ClearCompletedCommand = new RelayCommand(ClearCompleted, () => CompletedCount > 0);

        foreach (var todo in _storage.LoadTodos())
            Todos.Add(todo);

        UpdateCounts();
    }

    private void AddTodo()
    {
        var todo = new TodoItem { Title = NewItemTitle.Trim() };
        Todos.Add(todo);
        NewItemTitle = string.Empty;
        Save();
        UpdateCounts();
    }

    private void DeleteTodo(TodoItem? item)
    {
        if (item is null) return;

        Todos.Remove(item);
        Save();
        UpdateCounts();
    }

    private void ToggleCompleted(TodoItem? item)
    {
        if (item is null) return;

        item.IsCompleted = !item.IsCompleted;
        Save();
        UpdateCounts();
    }

    private void ClearCompleted()
    {
        for (int i = Todos.Count - 1; i >= 0; i--)
        {
            if (Todos[i].IsCompleted)
                Todos.RemoveAt(i);
        }

        Save();
        UpdateCounts();
    }

    private void Save() => _storage.SaveTodos(Todos);

    private void UpdateCounts()
    {
        TotalCount = Todos.Count;
        CompletedCount = Todos.Count(t => t.IsCompleted);
    }
}
