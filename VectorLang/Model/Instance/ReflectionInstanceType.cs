using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace VectorLang.Model;

internal sealed class ReflectionInstanceType : InstanceType
{
    private readonly Type InstanceReflectionType;

    private readonly Dictionary<string, PropertyInfo> _FieldProperties = new();

    private static readonly Dictionary<Type, ReflectionInstanceType> _CachedInstanceTypes = new();

    private ReflectionInstanceType(string name, Type instanceReflectionType)
        : base(name)
    {
        InstanceReflectionType = instanceReflectionType;
    }

    public static ReflectionInstanceType Of<TInstance>(string name) where TInstance : ReflectionInstance
    {
        return new(name, typeof(TInstance));
    }

    public Instance GetInstanceField(ReflectionInstance instance, string fieldName)
    {
        Debug.Assert(instance.GetType() == InstanceReflectionType);

        return (_FieldProperties[fieldName].GetValue(instance) as Instance)!;
    }

    protected override void DefineMembersInternal()
    {
        var instanceMembers = InstanceReflectionType.GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        foreach (var member in instanceMembers)
        {
            switch (member)
            {
                case PropertyInfo propertyInfo:
                    if (member.GetCustomAttribute<InstanceFieldAttribute>() is { FieldName: var fieldName })
                    {
                        DefineField(fieldName, GetInstanceTypeOf(propertyInfo.PropertyType));
                        _FieldProperties[fieldName] = propertyInfo;
                    }
                    break;

                case MethodInfo methodInfo:
                    if (member.GetCustomAttribute<InstanceMethodAttribute>() is { MethodName: var methodName })
                    {
                        DefineMethod(methodName, GetSignatureFromMethod(methodInfo), (thisInstance, arguments) =>
                        {
                            return (methodInfo.Invoke(thisInstance, arguments) as Instance)!;
                        });
                    }

                    if (member.GetCustomAttribute<InstanceOperatorAttribute>() is not null)
                    {
                        if (MethodNameToUnaryOperator(methodInfo.Name) is { } unaryOperator)
                        {
                            DefineOperator(unaryOperator, GetInstanceTypeOf(methodInfo.ReturnType), thisInstance =>
                            {
                                return (methodInfo.Invoke(null, new[] { thisInstance }) as Instance)!;
                            });
                        }

                        if (MethodNameToBinaryOperator(methodInfo.Name) is { } binaryOperator)
                        {
                            var returnInstanceType = GetInstanceTypeOf(methodInfo.ReturnType);
                            var rightInstanceType = GetInstanceTypeOf(methodInfo.GetParameters()[1].ParameterType);
                            DefineOperator(binaryOperator, returnInstanceType, rightInstanceType, (thisInstance, rightInstance) =>
                            {
                                return (methodInfo.Invoke(null, new[] { thisInstance, rightInstance }) as Instance)!;
                            });
                        }
                    }
                    break;
            }
        }
    }

    public static ReflectionInstanceType GetInstanceTypeOf(Type instanceReflectionType)
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

    private static CallSignature GetSignatureFromMethod(MethodInfo methodInfo)
    {
        var returnInstanceType = GetInstanceTypeOf(methodInfo.ReturnType);

        var parameters = methodInfo.GetParameters()
            .Where(param => param is { Name: not null, IsOut: false, IsIn: false })
            .Select(param => (param.Name!, GetInstanceTypeOf(param.ParameterType) as InstanceType));

        return new CallSignature(returnInstanceType, parameters.ToArray());
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
