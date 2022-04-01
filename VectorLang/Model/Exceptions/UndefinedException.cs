using System;

namespace VectorLang.Model;

public sealed class UndefinedException : Exception
{
    public UndefinedException(string value) : base($"{value} is undefined") { }

    internal static UndefinedException TypeMember(InstanceType type, string fieldOrMethod) => new($"field or method {type.FormatMember(fieldOrMethod)}");

    internal static UndefinedException TypeMember(InstanceType type, string method, params InstanceType[] argumentTypes) => new($"method {type.FormatMember(method, argumentTypes)}");

    internal static UndefinedException TypeMember(InstanceType type, UnaryOperator unaryOperator) => new($"unary operator {type.FormatMember(unaryOperator)}");

    internal static UndefinedException TypeMember(InstanceType type, BinaryOperator binaryOperator, InstanceType rightType) => new($"binary operator {type.FormatMember(binaryOperator, rightType)}");
}
