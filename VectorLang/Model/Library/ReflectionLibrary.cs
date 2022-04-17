using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace VectorLang.Model;

internal class ReflectionLibrary : Library
{
    protected sealed override void DefineItemsInternal()
    {
        foreach (var (constantName, field) in GetReflectionConstants())
        {
            DefineConstant(constantName, (field.GetValue(null) as Instance)!);
        }

        foreach (var (functionName, method) in GetReflectionMethods())
        {
            DefineFunction(ReflectionFunction.FromMethod(functionName, CreateMethodDelegate(method)));
        }
    }

    private IEnumerable<(string, FieldInfo)> GetReflectionConstants()
    {
        var fields = GetType()
            .GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
            .Where(field => field.IsInitOnly && field.FieldType.IsSubclassOf(typeof(Instance)));

        foreach (var field in fields)
        {
            if (field.GetCustomAttribute<ReflectionConstantAttribute>() is { ConstantName: var constantName })
            {
                yield return (constantName, field);
            }
        }
    }

    private IEnumerable<(string, MethodInfo)> GetReflectionMethods()
    {
        var methods = GetType()
            .GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

        foreach (var method in methods)
        {
            if (method.GetCustomAttribute<ReflectionFunctionAttribute>() is { FunctionName: var functionName })
            {
                yield return (functionName, method);
            }
        }
    }

    private Delegate CreateMethodDelegate(MethodInfo methodInfo)
    {
        Debug.Assert(methodInfo.ReturnType != typeof(void));

        var delegateType = Expression.GetFuncType(methodInfo.GetParameters()
            .Select(p => p.ParameterType)
            .Append(methodInfo.ReturnType)
            .ToArray());

        return methodInfo.IsStatic
            ? Delegate.CreateDelegate(delegateType, methodInfo)
            : Delegate.CreateDelegate(delegateType, this, methodInfo.Name);
    }
}
