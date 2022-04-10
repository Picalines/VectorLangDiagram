using System.Collections.Generic;

namespace VectorLang.Model;

internal abstract class FunctionLibrary
{
    private readonly List<Function> _Functions = new();

    public IReadOnlyList<Function> Functions => _Functions;

    protected void AddFunction(Function function)
    {
        _Functions.Add(function);
    }
}
