using System.Collections.Generic;
using System.Linq;
using VectorLang.Model;
using VectorLang.Parsing;
using VectorLang.SyntaxTree;
using VectorLang.Tokenization;

namespace VectorLang.Compilation;

public static class ProgramCompiler
{
    private static readonly List<InstanceType> _InstanceTypes = new()
    {
        NumberInstance.InstanceType,
        StringInstance.InstanceType,
        VectorInstance.InstanceType,
        ColorInstance.InstanceType,
    };

    public static CompiledProgram Compile(string code)
    {
        var tokens = Tokenizer.Tokenize(code);

        var syntaxTree = ProgramParser.Program.Parse(tokens);

        if (!syntaxTree.IsSuccessfull)
        {
            throw ProgramException.At(syntaxTree.Remainder.Selection, syntaxTree.ErrorMessage);
        }

        return Compile(syntaxTree.Value);
    }

    internal static CompiledProgram Compile(Program program)
    {
        var symbols = new SymbolTable();

        DefineInstanceTypes(symbols);

        DefineUserFunctions(program, symbols);

        CompileUserFunctions(symbols);

        var compiledPlots = CompilePlots(program, symbols);

        return new CompiledProgram(compiledPlots);
    }

    private static List<CompiledPlot> CompilePlots(Program program, SymbolTable symbols)
    {
        var compiledPlots = new List<CompiledPlot>();

        foreach (var plot in program.Definitions.OfType<PlotDefinition>())
        {
            compiledPlots.Add(PlotCompiler.Compile(symbols, plot));
        }

        return compiledPlots;
    }

    private static void CompileUserFunctions(SymbolTable symbols)
    {
        foreach (var functionSymbol in symbols.LocalSymbols.OfType<FunctionSymbol>())
        {
            if (functionSymbol is { Function: UserFunction userFunction })
            {
                userFunction.Compile();
            }
        }
    }

    private static void DefineUserFunctions(Program program, SymbolTable symbols)
    {
        foreach (var function in program.Definitions.OfType<FunctionDefinition>())
        {
            var userFunction = UserFunctionCompiler.Compile(symbols, function);

            symbols.Insert(new FunctionSymbol(userFunction));
        }
    }

    private static void DefineInstanceTypes(SymbolTable symbols)
    {
        foreach (var instanceType in _InstanceTypes)
        {
            if (!instanceType.IsDefined)
            {
                instanceType.DefineMembers();
            }

            symbols.Insert(new InstanceTypeSymbol(instanceType));
        }
    }
}
