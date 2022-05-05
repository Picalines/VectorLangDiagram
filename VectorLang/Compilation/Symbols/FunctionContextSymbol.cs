namespace VectorLang.Compilation;

internal sealed record FunctionContextSymbol() : Symbol(Name)
{
    public new const string Name = "#functionContext";

    private int _VariableAddress = 0;

    public int GenerateVariableAddress() => _VariableAddress++;
}
