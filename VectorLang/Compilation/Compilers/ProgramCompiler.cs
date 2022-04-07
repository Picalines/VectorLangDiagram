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
        VoidInstance.InstanceType,
    };

    public static CompiledProgram? Compile(string code, out IReadOnlyList<Report> reports)
    {
        var context = new CompilationContext();
        reports = context.Reporter.Reports;

        var tokens = Tokenizer.Tokenize(code);

        IParseResult<Program> syntaxTree;

        try
        {
            syntaxTree = ProgramParser.Program.Parse(tokens);
        }
        catch (UnknownTokenException unknownTokenException)
        {
            context.Reporter.ReportError(new(unknownTokenException.Location, 1), unknownTokenException.Message);
            return null;
        }

        if (!syntaxTree.IsSuccessfull)
        {
            var errorMessage = syntaxTree.ErrorMessage;

            if (syntaxTree.Expectations.Any())
            {
                errorMessage += "; expected " + string.Join(" or ", syntaxTree.Expectations);
            }

            context.Reporter.ReportError(syntaxTree.Remainder.Selection, errorMessage);
            return null;
        }

        var compiledProgram = Compile(context, syntaxTree.Value);

        if (context.Reporter.AnyErrors())
        {
            return null;
        }

        return compiledProgram;
    }

    internal static CompiledProgram Compile(CompilationContext context, Program program)
    {
        DefineInstanceTypes(context.Symbols);

        DefineUserFunctions(program, context);

        CompileUserFunctions(context.Symbols);

        var compiledPlots = CompilePlots(program, context);

        return new CompiledProgram(compiledPlots);
    }

    private static List<CompiledPlot> CompilePlots(Program program, CompilationContext context)
    {
        var compiledPlots = new List<CompiledPlot>();

        foreach (var plot in program.Definitions.OfType<PlotDefinition>())
        {
            compiledPlots.Add(PlotCompiler.Compile(context, plot));
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

    private static void DefineUserFunctions(Program program, CompilationContext context)
    {
        foreach (var function in program.Definitions.OfType<FunctionDefinition>())
        {
            var userFunction = UserFunctionCompiler.Compile(context, function);

            if (userFunction is not null)
            {
                context.Symbols.Insert(new FunctionSymbol(userFunction));
            }
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
