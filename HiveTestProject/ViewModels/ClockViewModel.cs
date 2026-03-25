using System.Windows.Threading;

namespace HiveTestProject.ViewModels;

public class ClockViewModel : BaseViewModel
{
    private string _currentTime = string.Empty;
    private string _currentDate = string.Empty;
    private string _dayOfWeek = string.Empty;
    private readonly DispatcherTimer _timer;

    public string CurrentTime
    {
        get => _currentTime;
        private set => SetProperty(ref _currentTime, value);
    }

    public string CurrentDate
    {
        get => _currentDate;
        private set => SetProperty(ref _currentDate, value);
    }

    public string DayOfWeek
    {
        get => _dayOfWeek;
        private set => SetProperty(ref _dayOfWeek, value);
    }

    public ClockViewModel()
    {
        UpdateTime();
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += (_, _) => UpdateTime();
        _timer.Start();
    }

    private void UpdateTime()
    {
        var now = DateTime.Now;
        CurrentTime = now.ToString("HH:mm:ss");
        CurrentDate = now.ToString("MMMM dd, yyyy");
        DayOfWeek = now.ToString("dddd");
    }
}
