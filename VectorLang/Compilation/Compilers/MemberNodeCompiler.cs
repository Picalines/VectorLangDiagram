using System.Linq;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class MemberNodeCompiler
{
    public static CompiledExpression Compile(SymbolTable symbols, MemberNode memberNode)
    {
        var fieldName = memberNode.Member;

        var (instanceType, instanceInstructions) = ValueExpressionCompiler.Compile(symbols, memberNode.Object);

        if (!instanceType.Fields.TryGetValue(fieldName, out var fieldType))
        {
            throw ProgramException.At(memberNode.MemberToken.Selection, UndefinedException.TypeMember(instanceType, fieldName));
        }

        return new CompiledExpression(
            Type: fieldType,
            Instructions: instanceInstructions.Append(new GetFieldInstruction(fieldName))
        );
    }
}
