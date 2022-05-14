using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Components;

namespace VectorLangDiagram.Shared;

public sealed class ProgramFileManager : ComponentBase
{
    private const string AppName = "VectorLangDiagram";

    [Parameter]
    public EventCallback<string> OnFileOpened { get; set; }

    [Parameter]
    public EventCallback OnFileCreated { get; set; }

    private static readonly FileFilter[] _ProgramFileFilters = new[]
    {
        new FileFilter() { Name = "VectorLang Program", Extensions = new[] { "vl" } },
    };

    private string? _CurrentPath;

    private bool _FileUnsaved = false;

    public string? CurrentPath
    {
        get => _CurrentPath;
        set
        {
            _CurrentPath = value;
            UpdateWindowTitle();
        }
    }

    public bool FileUnsaved
    {
        get => _FileUnsaved;
        set
        {
            _FileUnsaved = value;
            UpdateWindowTitle();
        }
    }

    public async Task Create()
    {
        CurrentPath = null;
        FileUnsaved = true;

        await OnFileCreated.InvokeAsync();
    }

    public async Task TryOpen()
    {
        var selectedPaths = await Electron.Dialog.ShowOpenDialogAsync(MainWindow, new OpenDialogOptions()
        {
            Filters = _ProgramFileFilters,
            DefaultPath = CurrentPath,
        });

        if (selectedPaths.FirstOrDefault() is not string programPath)
        {
            return;
        }

        CurrentPath = programPath;
        FileUnsaved = false;

        var program = await File.ReadAllTextAsync(programPath);

        await OnFileOpened.InvokeAsync(program);
    }

    public async Task TrySave(string content)
    {
        var savedToPath = CurrentPath ?? await Electron.Dialog.ShowSaveDialogAsync(MainWindow, new SaveDialogOptions()
        {
            Filters = _ProgramFileFilters,
            DefaultPath = CurrentPath,
        });

        if (savedToPath.Length == 0)
        {
            return;
        }

        await File.WriteAllTextAsync(savedToPath, content);

        CurrentPath = savedToPath;
        FileUnsaved = false;
    }

    private static BrowserWindow MainWindow
    {
        get => Electron.WindowManager.BrowserWindows.First();
    }

    private void UpdateWindowTitle()
    {
        var displayPath = CurrentPath is null
            ? "unnamed"
            : Path.GetFileName(CurrentPath);

        if (FileUnsaved)
        {
            displayPath += '*';
        }

        MainWindow.SetTitle($"{AppName} - {displayPath}");
    }
}
