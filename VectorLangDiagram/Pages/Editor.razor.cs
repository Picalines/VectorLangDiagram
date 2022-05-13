using Microsoft.AspNetCore.Components;
using VectorLang.Compilation;
using VectorLang.Diagnostics;
using VectorLang.Model;
using VectorLang.Tokenization;
using VectorLangDiagram.Shared;

using Timer = System.Timers.Timer;

namespace VectorLangDiagram.Pages;

public partial class Editor : ComponentBase
{
    private EditorWindows _ShownWindows = EditorWindows.Code | EditorWindows.Diagram;

    private readonly Stack<PlottedVector> _PlottedVectors = new();

    private readonly Timer _RecompileTimer = new(250);

    private CodeEditor _CodeEditor = null!;

    private Diagnoser? _CodeDiagnoser;

    private ExecutableProgram? _ExecutableProgram;

    private string? _ErrorMessage;

    protected override void OnInitialized()
    {
        base.OnInitialized();

        _RecompileTimer.AutoReset = false;
        _RecompileTimer.Elapsed += async (_, _) => await InvokeAsync(OnRecompileTimerElapsed);

        _RecompileTimer.Start();
    }

    private void RenderVector(PlottedVector vector)
    {
        _PlottedVectors.Push(vector);
    }

    private async Task CompileProgram()
    {
        if (_ExecutableProgram is not null)
        {
            _ExecutableProgram.VectorPlotted -= RenderVector;
        }

        _ExecutableProgram = ProgramCompiler.Compile(_CodeEditor.Value, out _CodeDiagnoser);

        int errorsCount = 0;

        foreach (var report in _CodeDiagnoser.Reports)
        {
            await _CodeEditor.AddReportAsync(report);

            if (report.Severity is ReportSeverity.Error)
            {
                errorsCount++;
            }
        }

        if (errorsCount > 0)
        {
            _ErrorMessage = $"{errorsCount} compilation errors";
        }

        if (_ExecutableProgram is not null)
        {
            _ExecutableProgram.VectorPlotted += RenderVector;
        }

        StateHasChanged();
    }

    private void ExecuteProgram()
    {
        if (_ExecutableProgram is null)
        {
            return;
        }

        _ErrorMessage = null;

        try
        {
            _PlottedVectors.Clear();
            _ExecutableProgram.Execute();
        }
        catch (RuntimeException runtimeException)
        {
            _ErrorMessage = runtimeException.Message;

            if (runtimeException.Selection is { } selection)
            {
                _ErrorMessage = $"{selection}: {_ErrorMessage}";
            }
        }

        StateHasChanged();
    }

    private IReadOnlyList<Completion> ProvideCompletions(TextLocation location)
    {
        if (_CodeDiagnoser is not null)
        {
            return _CodeDiagnoser.GetCompletions(location);
        }

        return Array.Empty<Completion>();
    }

    private async Task OnCodeEdited(string code)
    {
        await _CodeEditor.ClearReportsAsync();

        _ErrorMessage = null;

        _RecompileTimer.Stop();
        _RecompileTimer.Start();
    }

    private async Task OnRecompileTimerElapsed()
    {
        await CompileProgram();

        if (_ExecutableProgram is not null)
        {
            ExecuteProgram();
        }
    }
}
