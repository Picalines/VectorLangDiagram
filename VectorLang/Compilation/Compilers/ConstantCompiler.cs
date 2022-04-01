using System;
using System.Collections.Generic;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class ConstantCompiler
{
    public static IEnumerable<Instruction> Compile(ConstantNode constant) => constant switch
    {
        NumberNode { Value: var number } => new PushNumberInstruction(new NumberInstance(number)).Yield(),

        StringNode { Value: var str } => new PushStringInstruction(new StringInstance(str)).Yield(),

        ColorNode { R: var r, G: var g, B: var b } => new PushColorInstruction(new ColorInstance(r, g, b)).Yield(),

        _ => throw new NotImplementedException(),
    };
}
