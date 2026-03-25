# HiveTestProject — Personal Dashboard

## Overview
A modular personal dashboard WPF application serving as a foundation for an agent management system. Features a glassmorphism UI with three Phase 1 modules: Clock, Quick Notes, and Calculator.

## Tech Stack
- **.NET 10.0** (Windows target)
- **WPF** (Windows Presentation Foundation)
- **C#** with nullable reference types and implicit usings enabled
- No external NuGet packages — all functionality is built-in

## Build & Run
**Important:** Running in WSL2 — use the Windows dotnet.exe, not a Linux `dotnet`.
```bash
"/mnt/c/Program Files/dotnet/dotnet.exe" build HiveTestProject/HiveTestProject.csproj
"/mnt/c/Program Files/dotnet/dotnet.exe" run --project HiveTestProject/HiveTestProject.csproj
```

## Architecture
- **Pattern:** Full MVVM (Model-View-ViewModel)
- **Base classes:** `BaseViewModel` (INotifyPropertyChanged with `SetProperty<T>`), `RelayCommand` (ICommand)
- **Data binding:** All UI state flows through ViewModels, no logic in code-behind (except window chrome handlers)
- **Storage:** JSON file persistence via `JsonStorageService` → `%AppData%/HiveTestProject/notes.json`

## Project Structure
```
HiveTestProject.slnx                     # VS solution (modern slnx format)
HiveTestProject/
  HiveTestProject.csproj                  # Project file — OutputType: WinExe
  App.xaml / App.xaml.cs                  # Application entry, merges GlassTheme
  MainWindow.xaml / MainWindow.xaml.cs    # Custom chrome window, dashboard grid
  AssemblyInfo.cs                         # Assembly/theme metadata
  Themes/
    GlassTheme.xaml                       # Glassmorphism resource dictionary
  Models/
    NoteItem.cs                           # Note data model
  ViewModels/
    BaseViewModel.cs                      # INPC base class
    RelayCommand.cs                       # ICommand implementation
    MainViewModel.cs                      # Root VM — owns Clock, Notes, Calculator VMs
    ClockViewModel.cs                     # DispatcherTimer, time/date properties
    NotesViewModel.cs                     # CRUD notes, JSON persistence
    CalculatorViewModel.cs                # Arithmetic logic, display state
  Views/
    ClockWidget.xaml(.cs)                 # Clock UserControl
    NotesWidget.xaml(.cs)                 # Notes UserControl
    CalculatorWidget.xaml(.cs)            # Calculator UserControl
  Services/
    JsonStorageService.cs                 # JSON file read/write
  Converters/                             # (reserved for value converters)
```

## Design System — Glassmorphism

### Color Palette
| Token            | Value                            |
|------------------|----------------------------------|
| Background       | `#1a1a2e` → `#16213e` → `#0f3460` (diagonal gradient) |
| Accent           | `#e94560` (coral/pink)           |
| Text primary     | `#ffffff`                        |
| Text secondary   | `#b0b0c0`                        |
| Panel fill       | `rgba(255,255,255, 0.08)`        |
| Panel border     | `rgba(255,255,255, 0.15)`        |
| Button hover     | `rgba(255,255,255, 0.12)`        |
| Button pressed   | `rgba(255,255,255, 0.20)`        |

### Panel Style
- Corner radius: 12px
- Padding: 16px
- Border: 1px `PanelBorder`
- Drop shadow: BlurRadius=20, Opacity=0.3, black
- All interactive elements use rounded corners (8px buttons, 6px inputs/list items)

### Window
- **Custom chrome** — `WindowStyle=None`, `AllowsTransparency=True`
- **Size:** 900x600 (compact), min 750x500, resizable with grip
- **Title bar:** Custom with Segoe MDL2 Assets icons for minimize/maximize/close
- Double-click title bar toggles maximize

### Key Styles (in GlassTheme.xaml)
| Style Key          | Target     | Usage                        |
|--------------------|------------|------------------------------|
| `GlassPanel`       | Border     | Module container             |
| `GlassButton`      | Button     | Standard button              |
| `AccentButton`     | Button     | Primary action (coral bg)    |
| `TitleBarButton`   | Button     | Window control buttons       |
| `CloseButton`      | Button     | Close button (red hover)     |
| `GlassTextBox`     | TextBox    | Text input fields            |
| `GlassListBox`     | ListBox    | List containers              |
| `GlassListBoxItem` | ListBoxItem| List items with hover/select |
| `HeaderText`       | TextBlock  | Module titles (16px semibold)|
| `BodyText`         | TextBlock  | Secondary text (13px)        |
| `LargeDisplay`     | TextBlock  | Clock time (48px light)      |

## Module Specs

### Clock (1x1 grid cell, top-left)
- Updates every 1 second via `DispatcherTimer`
- Displays: time (HH:mm:ss), day of week (accent color), full date

### Quick Notes (2x1, right side, full height)
- Left panel: scrollable note list with content preview + timestamp
- Right panel: multi-line text editor with live binding
- Add/delete buttons in header
- Auto-saves to JSON on every change
- Persists to `%AppData%/HiveTestProject/notes.json`

### Calculator (1x1, bottom-left)
- Standard arithmetic: +, -, *, /
- Operations: C (clear all), CE (clear entry), backspace, negate (+/-), decimal
- 4x5 button grid, operator buttons in accent color
- Division by zero shows "Error"

## Dashboard Grid Layout
```
┌──────────────┬────────────────────────────┐
│              │                            │
│    Clock     │                            │
│    (1x1)     │       Quick Notes          │
│              │       (2x1, full height)   │
├──────────────┤                            │
│              │                            │
│  Calculator  │                            │
│    (1x1)     │                            │
│              │                            │
└──────────────┴────────────────────────────┘
```

## Conventions
- Solution uses `.slnx` format (not legacy `.sln`)
- Standard Visual Studio .gitignore is in place
- All new modules should be self-contained UserControls in `Views/` with a corresponding ViewModel
- ViewModels go in `ViewModels/`, models in `Models/`, services in `Services/`
- No external NuGet packages unless absolutely necessary
- No tests or CI/CD yet

## Roadmap
- **Phase 2:** System Info module, To-Do List module
- **Phase 3:** Weather widget (first API dependency), App Launcher
- **Future:** Drag-to-rearrange grid, module resize handles, settings panel
