using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VectorLang.Model;

internal abstract class Library
{
    private readonly List<Function> _Functions = new();

    private readonly Dictionary<string, Instance> _Constants = new();

    public IReadOnlyList<Function> Functions => _Functions;

    public IReadOnlyDictionary<string, Instance> Constants => _Constants;

    public bool IsDefined { get; private set; } = false;

    protected abstract void DefineItemsInternal();

    internal void DefineItems()
    {
        AssertNotDefined();

        try
        {
            DefineItemsInternal();
        }
        catch
        {
            _Functions.Clear();
            _Constants.Clear();
            throw;
        }

        IsDefined = true;
    }

    protected void DefineConstant(string name, Instance value)
    {
        AssertNotDefined();
        AssertItemNotDefined(name);

        _Constants.Add(name, value);
    }

    protected void DefineFunction(Function function)
    {
        AssertNotDefined();
        AssertItemNotDefined(function.Name);

        _Functions.Add(function);
    }

    [Conditional("DEBUG")]
    private void AssertNotDefined()
    {
        Debug.Assert(!IsDefined);
    }

    [Conditional("DEBUG")]
    private void AssertItemNotDefined(string name)
    {
        Debug.Assert(!_Constants.ContainsKey(name));
        Debug.Assert(!_Functions.Any(function => function.Name == name));
    }
}
