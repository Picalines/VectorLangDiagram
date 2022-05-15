using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Components;

namespace VectorLangDiagram.Shared;

public sealed class EditorMenu : ComponentBase
{
    [Parameter]
    public EventCallback<EditorWindows> OnViewSelected { get; set; }

    [Parameter]
    public EventCallback OnNewFilePressed { get; set; }

    [Parameter]
    public EventCallback OnFileOpenPressed { get; set; }

    [Parameter]
    public EventCallback OnFileSavePressed { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        Electron.Menu.SetApplicationMenu(new[]
        {
            CreateFileMenu(),
            CreateEditMenu(),
            CreateWindowMenu(),
        });
    }

    private MenuItem CreateFileMenu() => new()
    {
        Label = "File",
        Submenu = new MenuItem[]
        {
            new() { Label = "New", Accelerator = "CommandOrControl+N", Click = () => InvokeAsync(OnNewFilePressed.InvokeAsync) },
            new() { Label = "Open", Accelerator = "CommandOrControl+O", Click = () => InvokeAsync(OnFileOpenPressed.InvokeAsync) },
            new() { Label = "Save", Accelerator = "CommandOrControl+S", Click = () => InvokeAsync(OnFileSavePressed.InvokeAsync) },
            new() { Type = MenuType.separator },
            new() { Role = MenuRole.quit, Accelerator = "Alt+F4" },
        },
    };

    private static MenuItem CreateEditMenu() => new()
    {
        Role = MenuRole.editMenu,
        Submenu = new MenuItem[]
        {
            new() { Role = MenuRole.undo },
            new() { Role = MenuRole.redo },
        }
    };

    private MenuItem CreateWindowMenu() => new()
    {
        Role = MenuRole.windowMenu,
        Submenu = new[]
        {
            new MenuItem() { Role = MenuRole.togglefullscreen },

            CreateViewMenu(),

#if DEBUG
            new MenuItem() { Role = MenuRole.toggledevtools },
            new MenuItem() { Role = MenuRole.reload },
#endif
        },
    };

    private MenuItem CreateViewMenu()
    {
        Action SelectViewMode(EditorWindows windows)
        {
            return () => InvokeAsync(() => OnViewSelected.InvokeAsync(windows));
        }

        return new()
        {
            Label = "View",
            Submenu = new MenuItem[]
            {
                new() { Click = SelectViewMode(EditorWindows.Code | EditorWindows.Diagram), Label = "Code and Diagram" },
                new() { Click = SelectViewMode(EditorWindows.Code), Label = "Code only" },
                new() { Click = SelectViewMode(EditorWindows.Diagram), Label = "Diagram only" },
            }
        };
    }
}
