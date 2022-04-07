using System.Linq;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class MemberNodeCompiler
{
    public static CompiledExpression Compile(CompilationContext context, MemberNode memberNode)
    {
        var fieldName = memberNode.Member;

        var (instanceType, instanceInstructions) = ValueExpressionCompiler.Compile(context, memberNode.Object);

        if (!instanceType.Fields.TryGetValue(fieldName, out var fieldType))
        {
            context.Reporter.ReportError(memberNode.MemberToken.Selection, ReportMessage.UndefinedTypeMember(instanceType, fieldName));
            return CompiledExpression.Invalid;
        }

        return new(
            fieldType,
            instanceInstructions.Append(new GetFieldInstruction(fieldName))
        );
    }
}
