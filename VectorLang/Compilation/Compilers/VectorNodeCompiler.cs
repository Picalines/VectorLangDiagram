using System.Linq;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class VectorNodeCompiler
{
    public static CompiledExpression Compile(SymbolTable symbols, VectorNode vectorNode)
    {
        var compiledX = ValueExpressionCompiler.Compile(symbols, vectorNode.X);
        compiledX.AssertIsAssignableTo(NumberInstance.InstanceType, vectorNode.X.Selection);

        var compiledY = ValueExpressionCompiler.Compile(symbols, vectorNode.Y);
        compiledY.AssertIsAssignableTo(NumberInstance.InstanceType, vectorNode.Y.Selection);

        return new(
            Type: VectorInstance.InstanceType,
            Instructions: compiledX.Instructions
                .Concat(compiledY.Instructions)
                .Append(CreateVectorInstruction.Instance)
        );
    }
}
