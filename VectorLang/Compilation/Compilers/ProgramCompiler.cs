using System.Collections.Generic;
using System.Linq;
using VectorLang.Diagnostics;
using VectorLang.Model;
using VectorLang.Parsing;
using VectorLang.SyntaxTree;
using VectorLang.Tokenization;

namespace VectorLang.Compilation;

public static class ProgramCompiler
{
    private const string MainFunctionName = "main";

    private static readonly List<InstanceType> _InstanceTypes = new()
    {
        NumberInstance.InstanceType,
        BooleanInstance.InstanceType,
        VectorInstance.InstanceType,
        ColorInstance.InstanceType,
        VoidInstance.InstanceType,
    };

    private static readonly List<Library> _Libraries = new()
    {
        MathLibrary.Instance,
        ColorLibrary.Instance,
    };

    internal static IReadOnlyList<InstanceType> InstanceTypes
    {
        get => _InstanceTypes;
    }

    internal static IReadOnlyList<Library> Libraries
    {
        get => _Libraries;
    }

    public static ExecutableProgram? Compile(string code, out Diagnoser diagnoser)
    {
        var context = new CompilationContext();
        var (reporter, completionProvider, _) = context;

        diagnoser = new Diagnoser(reporter, completionProvider);

        var tokens = Tokenizer.Tokenize(code);

        IParseResult<Program> syntaxTree;

        try
        {
            syntaxTree = ProgramParser.Program.Parse(tokens);
        }
        catch (UnknownTokenException unknownTokenException)
        {
            reporter.ReportError(new(unknownTokenException.Location, 1), unknownTokenException.Message);

            return null;
        }

        if (!syntaxTree.IsSuccessfull)
        {
            var errorMessage = syntaxTree.ErrorMessage;

            if (syntaxTree.Expectations.Any())
            {
                errorMessage += "; expected " + string.Join(" or ", syntaxTree.Expectations);
            }

            reporter.ReportError(syntaxTree.Remainder.Selection, errorMessage);

            return null;
        }

        var executable = Compile(context, syntaxTree.Value);

        if (reporter.AnyErrors())
        {
            return null;
        }

        return executable;
    }

    internal static ExecutableProgram? Compile(CompilationContext context, Program program)
    {
        DefineInstanceTypes(context.Symbols);

        DefineLibraries(context.Symbols);

        CompileUserDefinitions(program, context);

        var plotLibrary = CreatePlotInterface(context.Symbols);

        CompileUserFunctions(context.Symbols);

        var mainFunction = CompileMainFunction(context, program);

        if (mainFunction is null)
        {
            return null;
        }

        var externalValues = context.Symbols
            .OfType<ExternalValueSymbol>()
            .ToDictionary(symbol => symbol.Name, symbol => symbol.ExternalValue);

        return new ExecutableProgram(plotLibrary, externalValues, mainFunction);
    }

    internal static void DefineInstanceTypesAndLibraries()
    {
        foreach (var instanceType in _InstanceTypes)
        {
            if (!instanceType.IsDefined)
            {
                instanceType.DefineMembers();
            }
        }

        foreach (var library in _Libraries)
        {
            if (!library.IsDefined)
            {
                library.DefineItems();
            }
        }
    }

    private static Function? CompileMainFunction(CompilationContext context, Program program)
    {
        if (!context.Symbols.TryLookup<FunctionSymbol>(MainFunctionName, out var mainFunctionSymbol))
        {
            context.Reporter.ReportError(new TextSelection(new(1, 1), 1), ReportMessage.MainFunctionNotFound);
            return null;
        }

        var mainFunction = mainFunctionSymbol.Function;

        if (mainFunction.Signature.Arguments.Any())
        {
            var mainFunctionDefinition = program.Definitions
                .OfType<FunctionDefinition>()
                .First(def => def.Name == MainFunctionName);

            context.Reporter.ReportError(mainFunctionDefinition.NameToken.Selection, ReportMessage.MainFunctionMustHaveNoArguments);
            return null;
        }

        return mainFunction;
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

    private static PlotLibrary CreatePlotInterface(SymbolTable symbols)
    {
        var plotLibrary = new PlotLibrary();

        DefineLibraryItems(symbols, plotLibrary);

        return plotLibrary;
    }

    private static void CompileUserDefinitions(Program program, CompilationContext context)
    {
        foreach (var constantDefinition in program.Definitions.OfType<ConstantDefinition>())
        {
            UserConstantCompiler.Compile(context, constantDefinition);
        }

        foreach (var externalDefinition in program.Definitions.OfType<ExternalValueDefinition>())
        {
            ExternalValueCompiler.Compile(context, externalDefinition);
        }

        foreach (var functionDefinition in program.Definitions.OfType<FunctionDefinition>())
        {
            UserFunctionCompiler.Compile(context, functionDefinition);
        }
    }

    private static void DefineLibraries(SymbolTable symbols)
    {
        foreach (var library in _Libraries)
        {
            DefineLibraryItems(symbols, library);
        }
    }

    private static void DefineLibraryItems(SymbolTable symbols, Library library)
    {
        if (!library.IsDefined)
        {
            library.DefineItems();
        }

        foreach (var (constantName, constantValue) in library.Constants)
        {
            symbols.Insert(new ConstantSymbol(constantName, constantValue));
        }

        foreach (var function in library.Functions)
        {
            symbols.Insert(new FunctionSymbol(function));
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
