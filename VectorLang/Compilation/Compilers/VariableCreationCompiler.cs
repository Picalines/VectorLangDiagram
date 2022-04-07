﻿using System;
using System.Linq;
using VectorLang.Model;
using VectorLang.SyntaxTree;

namespace VectorLang.Compilation;

internal static class VariableCreationCompiler
{
    public static CompiledExpression Compile(CompilationContext context, VariableCreationNode variableCreation)
    {
        var variableName = variableCreation.Variable.Identifier;

        bool redefinition = false;

        if (context.Symbols.ContainsLocal(variableName))
        {
            context.Reporter.ReportError(variableCreation.Variable.Selection, ReportMessage.RedefinedValue(variableName));
            redefinition = true;
        }

        if (!context.Symbols.TryLookup<FunctionContextSymbol>(out var functionContext))
        {
            throw new InvalidOperationException();
        }

        var compiledValue = ValueExpressionCompiler.Compile(context, variableCreation.ValueExpression);

        var variableAddress = functionContext.GenerateVariableAddress();

        if (!redefinition)
        {
            context.Symbols.Insert(new VariableSymbol(variableName, variableAddress, compiledValue.Type));
        }

        return new(
            compiledValue.Type,
            compiledValue.Instructions
                .Append(new StoreInstruction(variableAddress, false))
        );
    }
}
