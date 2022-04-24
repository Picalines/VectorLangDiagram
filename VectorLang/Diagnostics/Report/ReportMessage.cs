using VectorLang.Model;

namespace VectorLang.Diagnostics;

internal static class ReportMessage
{
    public const string MainFunctionNotFound = "entry function 'main' not found";

    public const string MainFunctionMustHaveNoArguments = "entry function 'main' must have no arguments";

    public static string NotAssignableType(InstanceType gotType, InstanceType expectedType) =>
        $"value of type {gotType} is not assignable to type {expectedType}";

    public static string WrongArgumentCount(int gotCount, int expectedCount) =>
        $"got {gotCount} arguments, but {expectedCount} expected";

    public static string WrongArgumentType(int argumentIndex, string argumentName, InstanceType gotType, InstanceType expectedType) =>
        $"{NotAssignableType(gotType, expectedType)} in argument #{argumentIndex + 1} '{argumentName}'";

    public static string RedefinedValue(string value) =>
        $"{value} is already defined";

    public static string UndefinedValue(string value) =>
        $"{value} is undefined";

    public static string UndefinedTypeMember(InstanceType type, string fieldOrMethod) =>
        UndefinedValue($"field or method '{type.FormatMember(fieldOrMethod)}'");

    public static string UndefinedTypeMember(InstanceType type, string method, params InstanceType[] argumentTypes) =>
        UndefinedValue($"method '{type.FormatMember(method, argumentTypes)}'");

    public static string UndefinedTypeMember(InstanceType type, UnaryOperator unaryOperator) =>
        UndefinedValue($"unary operator {type.FormatMember(unaryOperator)}");

    public static string UndefinedTypeMember(InstanceType type, BinaryOperator binaryOperator, InstanceType rightType) =>
        UndefinedValue($"binary operator {type.FormatMember(binaryOperator, rightType)}");

    public static string TypeIsNotAllowed(InstanceType notAllowedType) =>
        $"type {notAllowedType} is not allowed here";

    public static string NotCallableValue(string value) =>
        $"{value} is not callable";
}
