using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using VectorLang.Model;

namespace VectorLang.Compilation;

internal sealed class SymbolTable
{
    public readonly SymbolTable? Parent;

    private readonly Dictionary<string, Symbol> _Symbols = new();

    private SymbolTable(SymbolTable? parent)
    {
        Parent = parent;
    }

    public SymbolTable() : this(null)
    {
    }

    public SymbolTable Child() => new(this);

    public IEnumerable<Symbol> LocalSymbols => _Symbols.Values;

    public bool TryLookup(string name, [NotNullWhen(true)] out Symbol? symbol)
    {
        if (_Symbols.TryGetValue(name, out symbol))
        {
            return true;
        }

        if (Parent is not null)
        {
            return Parent.TryLookup(name, out symbol);
        }

        return symbol is not null;
    }

    public bool ContainsLocal(string name)
    {
        return _Symbols.ContainsKey(name);
    }

    public bool Contains(string name)
    {
        return ContainsLocal(name) || (Parent?.Contains(name) ?? false);
    }

    public void Insert(Symbol symbol)
    {
        Debug.Assert(!ContainsLocal(symbol.Name));

        _Symbols[symbol.Name] = symbol;
    }

    public bool TryLookup<TSymbol>(string name, [NotNullWhen(true)] out TSymbol? symbol)
        where TSymbol : Symbol
    {
        if (!TryLookup(name, out var foundSymbol))
        {
            symbol = null;
            return false;
        }

        symbol = foundSymbol as TSymbol;
        return symbol is not null;
    }

    public bool TryLookup<TSymbol>([NotNullWhen(true)] out TSymbol? symbol)
        where TSymbol : Symbol
    {
        symbol = _Symbols.Values.OfType<TSymbol>().FirstOrDefault();

        if (symbol is null && Parent is not null)
        {
            return Parent.TryLookup(out symbol);
        }

        return symbol is not null;
    }

    public TSymbol Lookup<TSymbol>(string name, string? undefinedValue = null)
        where TSymbol : Symbol
    {
        if (!TryLookup<TSymbol>(name, out var foundSymbol))
        {
            throw new UndefinedException(undefinedValue ?? name);
        }

        return foundSymbol;
    }
}
