using System.Linq;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class VectorNodeCompiler
{
    public static CompiledExpression Compile(CompilationContext context, VectorNode vectorNode)
    {
        var compiledX = ValueExpressionCompiler.Compile(context, vectorNode.X, NumberInstance.InstanceType);
        var compiledY = ValueExpressionCompiler.Compile(context, vectorNode.Y, NumberInstance.InstanceType);

        if (compiledX.IsInvalid || compiledY.IsInvalid)
        {
            return new(VectorInstance.InstanceType);
        }

        return new(
            VectorInstance.InstanceType,
            compiledX.Instructions
                .Concat(compiledY.Instructions)
                .Append(CreateVectorInstruction.Instance)
        );
    }
}
