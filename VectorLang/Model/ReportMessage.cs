﻿namespace VectorLang.Model;

internal static class ReportMessage
{
    public static string WrongArgumentCount(int gotCount, int expectedCount) =>
        $"got {gotCount} arguments, but {expectedCount} expected";

    public static string WrongArgumentType(int argumentIndex, string argumentName, InstanceType gotType, InstanceType expectedType) =>
        $"{gotType} is not assignable to {expectedType} in argument #{argumentIndex + 1} {argumentName}";

    public static string NotAssignableType(InstanceType gotType, InstanceType expectedType) =>
        $"value of type {gotType} is not assignable to type {expectedType}";

    public static string RedefinedValue(string value) =>
        $"{value} is already defined";

    public static string UndefinedValue(string value) =>
        $"{value} is undefined";

    internal static string UndefinedTypeMember(InstanceType type, string fieldOrMethod) =>
        UndefinedValue($"field or method {type.FormatMember(fieldOrMethod)}");

    internal static string UndefinedTypeMember(InstanceType type, string method, params InstanceType[] argumentTypes) =>
        UndefinedValue($"method {type.FormatMember(method, argumentTypes)}");

    internal static string UndefinedTypeMember(InstanceType type, UnaryOperator unaryOperator) =>
        UndefinedValue($"unary operator {type.FormatMember(unaryOperator)}");

    internal static string UndefinedTypeMember(InstanceType type, BinaryOperator binaryOperator, InstanceType rightType) =>
        UndefinedValue($"binary operator {type.FormatMember(binaryOperator, rightType)}");
}