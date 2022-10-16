using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    public static ReflectionInstanceType Of<TInstance>(string name) where TInstance : ReflectionInstance<TInstance>
    {
        return new(name, typeof(TInstance));
    }

    public Instance GetInstanceField<TInstance>(ReflectionInstance<TInstance> instance, string fieldName) where TInstance : ReflectionInstance<TInstance>
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
                else if (MethodNameToBinaryOperator(methodInfo.Name) is { } binaryOperator)
                {
                    var (returnType, arguments) = CallSignature.From(methodInfo);

                    Debug.Assert(arguments.Count == 2, invalidAttributeUsageMessage);

                    DefineOperator(binaryOperator, returnType, arguments[1].Type, (thisInstance, rightInstance) =>
                    {
                        return (methodInfo.InvokeAndRethrow(null, new[] { thisInstance, rightInstance }) as Instance)!;
                    });
                }
                else
                {
                    Debug.Fail($"{nameof(InstanceOperatorAttribute)} is not allowed on method '{methodInfo.Name}'");
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

        Debug.Assert(instanceReflectionType.BaseType?.GetGenericTypeDefinition() == typeof(ReflectionInstance<>), $"{instanceReflectionType} is not an {typeof(ReflectionInstance<>).Name}");

        // why nameof(ReflectionInstance<>.InstanceType) isn't a thing? :/
        var typeStaticField = instanceReflectionType.GetProperty("InstanceType", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

        Debug.Assert(typeStaticField is not null, "ReflectionInstance<>.InstanceType field is not found");

        instaceType = (typeStaticField.GetValue(null) as ReflectionInstanceType)!;
        _CachedInstanceTypes.Add(instanceReflectionType, instaceType);

        return instaceType;
    }

    private static UnaryOperator? MethodNameToUnaryOperator(string methodName) => methodName switch
    {
        "op_UnaryPlus" => UnaryOperator.Plus,
        "op_UnaryNegation" => UnaryOperator.Minus,
        "op_LogicalNot" => UnaryOperator.Not,
        _ => null,
    };

    private static BinaryOperator? MethodNameToBinaryOperator(string methodName) => methodName switch
    {
        "op_Addition" => BinaryOperator.Plus,
        "op_Subtraction" => BinaryOperator.Minus,
        "op_Multiply" => BinaryOperator.Multiply,
        "op_Division" => BinaryOperator.Divide,
        "op_Modulus" => BinaryOperator.Modulo,
        "op_LessThan" => BinaryOperator.Less,
        "op_LessThanOrEqual" => BinaryOperator.LessOrEqual,
        "op_GreaterThan" => BinaryOperator.Greater,
        "op_GreaterThanOrEqual" => BinaryOperator.GreaterOrEqual,
        "op_Equality" => BinaryOperator.Equals,
        "op_Inequality" => BinaryOperator.NotEquals,
        _ => null,
    };
}
