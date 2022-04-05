using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace VectorLang.Model;

internal sealed class ReflectionInstanceType : InstanceType
{
    private readonly Type _InstanceReflectionType;

    private readonly Dictionary<string, PropertyInfo> _FieldProperties = new();

    private static readonly Dictionary<Type, ReflectionInstanceType> _CachedInstanceTypes = new();

    private ReflectionInstanceType(string name, Type instanceReflectionType)
        : base(name)
    {
        _InstanceReflectionType = instanceReflectionType;
    }

    public static ReflectionInstanceType Of<TInstance>(string name) where TInstance : ReflectionInstance
    {
        return new(name, typeof(TInstance));
    }

    public Instance GetInstanceField(ReflectionInstance instance, string fieldName)
    {
        Debug.Assert(instance.GetType() == _InstanceReflectionType);

        return (_FieldProperties[fieldName].GetValue(instance) as Instance)!;
    }

    protected override void DefineMembersInternal()
    {
        const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

        var properties = _InstanceReflectionType.GetProperties(bindingFlags);

        foreach (var propertyInfo in properties)
        {
            if (propertyInfo.GetCustomAttribute<InstanceFieldAttribute>() is { FieldName: var fieldName })
            {
                DefineField(fieldName, From(propertyInfo.PropertyType));
                _FieldProperties[fieldName] = propertyInfo;
            }
        }

        var methodOrOperators = _InstanceReflectionType.GetMethods(bindingFlags);

        foreach (var methodInfo in methodOrOperators)
        {
            if (methodInfo.GetCustomAttribute<InstanceMethodAttribute>() is { MethodName: var langMethodName })
            {
                DefineMethod(langMethodName, CallSignature.From(methodInfo), (thisInstance, arguments) =>
                {
                    return (methodInfo.InvokeAndRethrow(thisInstance, arguments) as Instance)!;
                });
            }

            if (methodInfo.GetCustomAttribute<InstanceOperatorAttribute>() is not null)
            {
                const string invalidAttributeUsageMessage = $"{nameof(InstanceOperatorAttribute)} can be used only on operator declarations";

                if (MethodNameToUnaryOperator(methodInfo.Name) is { } unaryOperator)
                {
                    Debug.Assert(methodInfo.GetParameters().Length == 1, invalidAttributeUsageMessage);

                    DefineOperator(unaryOperator, From(methodInfo.ReturnType), thisInstance =>
                    {
                        return (methodInfo.InvokeAndRethrow(null, new[] { thisInstance }) as Instance)!;
                    });
                }

                if (MethodNameToBinaryOperator(methodInfo.Name) is { } binaryOperator)
                {
                    var (returnType, arguments) = CallSignature.From(methodInfo);

                    Debug.Assert(arguments.Count == 2, invalidAttributeUsageMessage);

                    DefineOperator(binaryOperator, returnType, arguments[1].Type, (thisInstance, rightInstance) =>
                    {
                        return (methodInfo.InvokeAndRethrow(null, new[] { thisInstance, rightInstance }) as Instance)!;
                    });
                }
            }
        }
    }

    public static ReflectionInstanceType From(Type instanceReflectionType)
    {
        if (_CachedInstanceTypes.TryGetValue(instanceReflectionType, out var instaceType))
        {
            return instaceType;
        }

        Debug.Assert(instanceReflectionType.IsSubclassOf(typeof(ReflectionInstance)), $"{instanceReflectionType} is not an {nameof(ReflectionInstance)}");

        var fields = instanceReflectionType.GetFields(BindingFlags.Public | BindingFlags.Static);

        var typeStaticField = fields
            .Where(field => field.IsDefined(typeof(ReflectionInstanceTypeAttribute)))
            .FirstOrDefault();

        Debug.Assert(typeStaticField is not null, $"{nameof(ReflectionInstance)} must have a public static InstanceType Type field with {nameof(ReflectionInstanceTypeAttribute)}");

        instaceType = (typeStaticField.GetValue(null) as ReflectionInstanceType)!;
        _CachedInstanceTypes.Add(instanceReflectionType, instaceType);

        return instaceType;
    }

    private static UnaryOperator? MethodNameToUnaryOperator(string methodName) => methodName switch
    {
        "op_UnaryPlus" => UnaryOperator.Plus,
        "op_UnaryNegation" => UnaryOperator.Minus,
        _ => null,
    };

    private static BinaryOperator? MethodNameToBinaryOperator(string methodName) => methodName switch
    {
        "op_Addition" => BinaryOperator.Plus,
        "op_Substraction" => BinaryOperator.Minus,
        "op_Multiply" => BinaryOperator.Multiply,
        "op_Division" => BinaryOperator.Divide,
        "op_Modulus" => BinaryOperator.Modulo,
        _ => null,
    };
}
