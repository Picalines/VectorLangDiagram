using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Components;

namespace VectorLangDiagram.Shared;

public sealed class EditorMenu : ComponentBase
{
    [Parameter]
    public EventCallback<EditorWindows> OnViewSelected { get; set; }

    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (!HybridSupport.IsElectronActive)
        {
            return;
        }

        Electron.Menu.SetApplicationMenu(new[]
        {
            CreateFileMenu(),
            CreateEditMenu(),
            CreateWindowMenu(),
        });
    }

    private static MenuItem CreateFileMenu() => new()
    {
        Label = "File",
        Submenu = new MenuItem[]
        {
            new() { Label = "Open", Accelerator = "CommandOrControl+O" },
            new() { Label = "Save", Accelerator = "CommandOrControl+S" },
            new() { Label = "Save as...", Accelerator = "CommandOrControl+Shift+S" },
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
