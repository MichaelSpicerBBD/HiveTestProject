using HiveTestProject.ViewModels;

namespace HiveTestProject.Models;

public class TodoItem : BaseViewModel
{
    private string _title = string.Empty;
    private bool _isCompleted;

    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public bool IsCompleted
    {
        get => _isCompleted;
        set => SetProperty(ref _isCompleted, value);
    }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
