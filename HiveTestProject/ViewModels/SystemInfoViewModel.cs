using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Threading;

namespace HiveTestProject.ViewModels;

public class SystemInfoViewModel : BaseViewModel
{
    private string _memoryUsedMb = string.Empty;
    private string _uptime = string.Empty;
    private string _processCount = string.Empty;
    private readonly DispatcherTimer _timer;

    // Static properties (set once in constructor)
    public string MachineName { get; }
    public string OsVersion { get; }
    public string ProcessorCount { get; }
    public string DotNetVersion { get; }

    // Dynamic properties (updated by timer)
    public string MemoryUsedMb
    {
        get => _memoryUsedMb;
        private set => SetProperty(ref _memoryUsedMb, value);
    }

    public string Uptime
    {
        get => _uptime;
        private set => SetProperty(ref _uptime, value);
    }

    public string ProcessCount
    {
        get => _processCount;
        private set => SetProperty(ref _processCount, value);
    }

    public SystemInfoViewModel()
    {
        MachineName = Environment.MachineName;
        OsVersion = RuntimeInformation.OSDescription;
        ProcessorCount = $"{Environment.ProcessorCount} cores";
        DotNetVersion = RuntimeInformation.FrameworkDescription;

        UpdateDynamicInfo();
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
        _timer.Tick += (_, _) => UpdateDynamicInfo();
        _timer.Start();
    }

    private void UpdateDynamicInfo()
    {
        var process = Process.GetCurrentProcess();
        var memoryMb = process.WorkingSet64 / (1024.0 * 1024.0);
        MemoryUsedMb = $"{memoryMb:F1} MB";

        var uptimeMs = Environment.TickCount64;
        var uptimeSpan = TimeSpan.FromMilliseconds(uptimeMs);
        Uptime = $"{(int)uptimeSpan.TotalDays}d {uptimeSpan.Hours}h {uptimeSpan.Minutes}m";

        try
        {
            ProcessCount = $"{Process.GetProcesses().Length}";
        }
        catch
        {
            ProcessCount = "N/A";
        }
    }
}
