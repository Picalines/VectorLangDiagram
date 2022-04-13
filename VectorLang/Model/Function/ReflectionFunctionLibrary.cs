using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace VectorLang.Model;

internal class ReflectionFunctionLibrary : FunctionLibrary
{
    public ReflectionFunctionLibrary()
    {
        foreach (var (functionName, method) in GetTargetMethods())
        {
            AddFunction(ReflectionFunction.FromMethod(functionName, CreateMethodDelegate(method)));
        }
    }

    private IEnumerable<(string, MethodInfo)> GetTargetMethods()
    {
        var methods = GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

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
