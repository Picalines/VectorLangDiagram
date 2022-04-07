using System;
using System.Collections.Generic;
using System.Diagnostics;
using VectorLang.Compilation;
using VectorLang.Model;

namespace VectorLang.Interpretation;

internal static partial class Interpreter
{
    public static Instance Interpret(IReadOnlyList<Instruction> instructions, params Instance[] arguments)
    {
        Debug.Assert(instructions.Count > 0);

        var context = new InterpretationContext(instructions);

        foreach (var argument in arguments)
        {
            context.PushToStack(argument);
        }

        return Interpret(context);
    }

    private static Instance Interpret(InterpretationContext context)
    {
        while (!context.AtEnd)
        {
            InterpretInstruction(context, out var jumped);

            if (!jumped)
            {
                context.MoveNext();
            }
        }

        return context.PopFromStack();
    }

    private static void InterpretInstruction(InterpretationContext context, out bool jumped)
    {
        jumped = false;

        switch (context.CurrentInstruction)
        {
            case PushInstruction { Instance: var value }:
            {
                context.PushToStack(value);
            }
            break;

            case PopInstruction:
            {
                context.PopFromStack();
            }
            break;

            case JumpInstruction { Delta: var delta }:
            {
                context.Jump(delta);
                jumped = true;
            }
            break;

            case StoreInstruction { Address: var destination, PopFromStack: var popFromStack }:
            {
                var value = popFromStack ? context.PopFromStack() : context.PeekStack();
                context.StoreValue(destination, value);
            }
            break;

            case LoadInstruction { Address: var source }:
            {
                context.PushToStack(context.LoadValue(source));
            }
            break;

            case GetFieldInstruction { FieldName: var fieldName }:
            {
                var thisInstance = context.PopFromStack();
                context.PushToStack(thisInstance.GetField(fieldName));
            }
            break;

            case CreateVectorInstruction:
            {
                var vectorY = (context.PopFromStack() as NumberInstance)!;
                var vectorX = (context.PopFromStack() as NumberInstance)!;
                context.PushToStack(new VectorInstance(vectorX, vectorY));
                break;
            }

            case CallFunctionInstruction { Function: var function, ArgumentCount: var argumentsCount }:
            {
                var arguments = PopArguments(context, argumentsCount);
                context.PushToStack(function.Call(arguments));
            }
            break;

            case CallMethodInstruction { Method: var method, ArgumentCount: var argumentsCount }:
            {
                var arguments = PopArguments(context, argumentsCount);
                var thisInstance = context.PopFromStack();
                context.PushToStack(method.Call(thisInstance, arguments));
            }
            break;

            case UnaryOperatorInstruction { Operator: var unaryOperator }:
            {
                var thisInstance = context.PopFromStack();
                context.PushToStack(unaryOperator.Call(thisInstance));
            }
            break;

            case BinaryOperatorInstruction { Operator: var binaryOperator }:
            {
                var rightInstance = context.PopFromStack();
                var leftInstance = context.PopFromStack();
                context.PushToStack(binaryOperator.Call(leftInstance, rightInstance));
            }
            break;
        }
    }

    private static Instance[] PopArguments(InterpretationContext context, int count)
    {
        if (count == 0)
        {
            return Array.Empty<Instance>();
        }

        Instance[] arguments = new Instance[count];

        for (int i = arguments.Length - 1; i >= 0; i--)
        {
            arguments[i] = context.PopFromStack();
        }

        return arguments;
    }
}
