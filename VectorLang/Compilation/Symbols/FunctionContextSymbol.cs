using VectorLang.Model;

namespace VectorLang.Compilation;

internal sealed record FunctionContextSymbol(InstanceType ReturnType) : Symbol(Name)
{
    public new const string Name = "#functionContext";

    private int _VariableAddress = 0;

    public int GenerateVariableAddress() => _VariableAddress++;
}
