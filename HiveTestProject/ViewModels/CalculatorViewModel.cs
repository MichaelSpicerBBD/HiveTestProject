using System.Globalization;
using System.Windows.Input;

namespace HiveTestProject.ViewModels;

public class CalculatorViewModel : BaseViewModel
{
    private string _display = "0";
    private double _currentValue;
    private string? _pendingOperation;
    private bool _isNewEntry = true;

    public string Display
    {
        get => _display;
        private set => SetProperty(ref _display, value);
    }

    public ICommand NumberCommand { get; }
    public ICommand OperatorCommand { get; }
    public ICommand EqualsCommand { get; }
    public ICommand ClearCommand { get; }
    public ICommand ClearEntryCommand { get; }
    public ICommand DecimalCommand { get; }
    public ICommand BackspaceCommand { get; }
    public ICommand NegateCommand { get; }

    public CalculatorViewModel()
    {
        NumberCommand = new RelayCommand(p => AppendDigit(p?.ToString() ?? "0"));
        OperatorCommand = new RelayCommand(p => SetOperator(p?.ToString() ?? "+"));
        EqualsCommand = new RelayCommand(Calculate);
        ClearCommand = new RelayCommand(Clear);
        ClearEntryCommand = new RelayCommand(ClearEntry);
        DecimalCommand = new RelayCommand(AppendDecimal);
        BackspaceCommand = new RelayCommand(Backspace);
        NegateCommand = new RelayCommand(Negate);
    }

    private void AppendDigit(string digit)
    {
        if (_isNewEntry)
        {
            Display = digit;
            _isNewEntry = false;
        }
        else
        {
            Display = Display == "0" ? digit : Display + digit;
        }
    }

    private void AppendDecimal()
    {
        if (_isNewEntry)
        {
            Display = "0.";
            _isNewEntry = false;
        }
        else if (!Display.Contains('.'))
        {
            Display += ".";
        }
    }

    private void SetOperator(string op)
    {
        if (!_isNewEntry)
        {
            Calculate();
        }

        _currentValue = double.Parse(Display, CultureInfo.InvariantCulture);
        _pendingOperation = op;
        _isNewEntry = true;
    }

    private void Calculate()
    {
        if (_pendingOperation is null) return;

        var input = double.Parse(Display, CultureInfo.InvariantCulture);
        var result = _pendingOperation switch
        {
            "+" => _currentValue + input,
            "-" => _currentValue - input,
            "*" => _currentValue * input,
            "/" => input != 0 ? _currentValue / input : double.NaN,
            _ => input
        };

        Display = double.IsNaN(result) ? "Error" : result.ToString(CultureInfo.InvariantCulture);
        _currentValue = result;
        _pendingOperation = null;
        _isNewEntry = true;
    }

    private void Clear()
    {
        Display = "0";
        _currentValue = 0;
        _pendingOperation = null;
        _isNewEntry = true;
    }

    private void ClearEntry()
    {
        Display = "0";
        _isNewEntry = true;
    }

    private void Backspace()
    {
        if (_isNewEntry || Display.Length <= 1 || Display == "Error")
        {
            Display = "0";
            _isNewEntry = true;
        }
        else
        {
            Display = Display[..^1];
        }
    }

    private void Negate()
    {
        if (Display != "0" && Display != "Error")
        {
            if (Display.StartsWith('-'))
                Display = Display[1..];
            else
                Display = "-" + Display;
        }
    }
}
