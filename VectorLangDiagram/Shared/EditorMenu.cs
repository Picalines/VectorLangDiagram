using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Components;

namespace VectorLangDiagram.Shared;

public sealed class EditorMenu : ComponentBase
{
    protected override void OnInitialized()
    {
        base.OnInitialized();

        if (!HybridSupport.IsElectronActive)
        {
            return;
        }

        Electron.Menu.SetApplicationMenu(new[]
        {
            new MenuItem()
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
            },

            new MenuItem()
            {
                Role = MenuRole.editMenu,
                Submenu = new MenuItem[]
                {
                    new() { Role = MenuRole.undo },
                    new() { Role = MenuRole.redo },
                }
            },

            new MenuItem()
            {
                Role = MenuRole.windowMenu,
                Submenu = new[]
                {
                    new MenuItem() { Role = MenuRole.togglefullscreen },

                    new MenuItem()
                    {
                        Label = "View",
                        Submenu = new MenuItem[]
                        {
                            new() { Type = MenuType.radio, Label = "Code and Diagram" },
                            new() { Type = MenuType.radio, Label = "Code only" },
                            new() { Type = MenuType.radio, Label = "Diagram only" },
                        }
                    },
                },
            },
        });
    }
}
