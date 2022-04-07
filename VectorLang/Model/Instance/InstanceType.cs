using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace VectorLang.Model;

internal abstract class InstanceType
{
    public string Name { get; }

    public bool IsDefined { get; private set; } = false;

    private readonly Dictionary<string, InstanceType> _Fields = new();

    private readonly Dictionary<string, InstanceMethod> _Methods = new();

    private readonly Dictionary<UnaryOperator, InstanceUnaryOperator> _UnaryOperators = new();

    private readonly Dictionary<(BinaryOperator, InstanceType), InstanceBinaryOperator> _BinaryOperators = new();

    public InstanceType(string name)
    {
        Name = name;
    }

    public IReadOnlyDictionary<string, InstanceType> Fields => _Fields;

    public IReadOnlyDictionary<string, InstanceMethod> Methods => _Methods;

    public IReadOnlyDictionary<UnaryOperator, InstanceUnaryOperator> UnaryOperators => _UnaryOperators;

    public IReadOnlyDictionary<(BinaryOperator, InstanceType), InstanceBinaryOperator> BinaryOperators => _BinaryOperators;

    public override string ToString() => Name;

    public bool IsAssignableTo(InstanceType otherType) => this == otherType;

    public string FormatMember(string fieldOrMethod) => $"{Name}.{fieldOrMethod}";

    public string FormatMember(string method, params InstanceType[] argumentTypes) => $"{Name}.{method}({string.Join(", ", argumentTypes.Select(type => type.ToString()))})";

    public string FormatMember(UnaryOperator unaryOperator) => unaryOperator.GetFormatted(Name);

    public string FormatMember(BinaryOperator binaryOperator, InstanceType rightType) => binaryOperator.GetFormatted(Name, rightType);

    protected abstract void DefineMembersInternal();

    internal void DefineMembers()
    {
        AssertNotDefined();

        try
        {
            DefineMembersInternal();
        }
        catch
        {
            _Fields.Clear();
            _Methods.Clear();
            _UnaryOperators.Clear();
            _BinaryOperators.Clear();
            throw;
        }

        IsDefined = true;
    }

    protected void DefineField(string name, InstanceType type)
    {
        AssertNotDefined();
        AssertMemberNotDefined(name);

        _Fields[name] = type;
    }

    protected void DefineMethod(string name, CallSignature signature, InstanceMethod.CallableDelegate callable)
    {
        AssertNotDefined();
        AssertMemberNotDefined(name);

        _Methods[name] = new InstanceMethod(this, signature, callable);
    }

    protected void DefineOperator(UnaryOperator unaryOperator, InstanceType returnType, InstanceUnaryOperator.CallableDelegate callable)
    {
        AssertNotDefined();

        Debug.Assert(!_UnaryOperators.ContainsKey(unaryOperator), $"unary operator {FormatMember(unaryOperator)} is already defined in type {this}");

        _UnaryOperators[unaryOperator] = new InstanceUnaryOperator(this, returnType, callable);
    }

    protected void DefineOperator(BinaryOperator binaryOperator, InstanceType returnType, InstanceType rightType, InstanceBinaryOperator.CallableDelegate callable)
    {
        AssertNotDefined();

        Debug.Assert(!_BinaryOperators.ContainsKey((binaryOperator, rightType)), $"binary operator {FormatMember(binaryOperator, rightType)} is already defined");

        _BinaryOperators[(binaryOperator, rightType)] = new InstanceBinaryOperator(this, returnType, rightType, callable);
    }

    [Conditional("DEBUG")]
    private void AssertNotDefined()
    {
        Debug.Assert(!IsDefined, $"type {this} is already defined");
    }

    [Conditional("DEBUG")]
    private void AssertMemberNotDefined(string name)
    {
        Debug.Assert(!_Fields.ContainsKey(name), $"field named '{name}' is already defined in type {this}");
        Debug.Assert(!_Methods.ContainsKey(name), $"method named '{name}' is already defined in type {this}");
    }
}
