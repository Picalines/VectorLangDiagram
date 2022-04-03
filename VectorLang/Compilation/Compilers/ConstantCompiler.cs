﻿using System;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class ConstantCompiler
{
    public static CompiledExpression Compile(ConstantNode constant) => constant switch
    {
        NumberNode { Value: var number } => new(NumberInstance.InstanceType, new PushNumberInstruction(new NumberInstance(number))),

        StringNode { Value: var str } => new(StringInstance.InstanceType, new PushStringInstruction(new StringInstance(str))),

        ColorNode { R: var r, G: var g, B: var b } => new(ColorInstance.InstanceType, new PushColorInstruction(new ColorInstance(r, g, b))),

        _ => throw new NotImplementedException(),
    };
}