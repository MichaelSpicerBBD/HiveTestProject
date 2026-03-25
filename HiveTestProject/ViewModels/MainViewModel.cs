namespace HiveTestProject.ViewModels;

public class MainViewModel : BaseViewModel
{
    public ClockViewModel Clock { get; } = new();
    public NotesViewModel Notes { get; }
    public CalculatorViewModel Calculator { get; } = new();

    public MainViewModel()
    {
        var storageService = new Services.JsonStorageService();
        Notes = new NotesViewModel(storageService);
    }
}
