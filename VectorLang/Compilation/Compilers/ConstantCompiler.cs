using System;
using VectorLang.Interpretation;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class ConstantCompiler
{
    public static CompiledExpression Compile(ConstantNode constant) => constant switch
    {
        NumberNode { Value: var number } => new(NumberInstance.InstanceType, new PushInstruction(NumberInstance.From(number))),

        BooleanNode { Value: var boolean } => new(BooleanInstance.InstanceType, new PushInstruction(BooleanInstance.From(boolean))),

        ColorNode { R: var r, G: var g, B: var b } => new(ColorInstance.InstanceType, new PushInstruction(new ColorInstance(r, g, b))),

        VoidNode => new(VoidInstance.InstanceType, new PushInstruction(VoidInstance.Instance)),

        _ => throw new NotImplementedException(),
    };
}
