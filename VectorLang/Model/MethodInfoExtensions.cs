using System.Reflection;

namespace VectorLang.Model;

internal static class MethodInfoExtensions
{
    public static object? InvokeAndRethrow(this MethodInfo methodInfo, object? obj, object?[]? parameters)
    {
        try
        {
            return methodInfo.Invoke(obj, parameters);
        }
        catch (TargetInvocationException targetInvocationException)
        {
            throw targetInvocationException.InnerException!;
        }
    }
}
