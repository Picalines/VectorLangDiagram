using System;
using System.Collections.Generic;
using System.Diagnostics;

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

    public bool IsAssignableTo(InstanceType otherType)
    {
        return this == otherType;
    }

    protected abstract void DefineMembersInternal();

    internal void DefineMembers()
    {
        Debug.Assert(!IsDefined);

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

    protected void DefineMethod<TThis>(string name, CallSignature signature, Func<TThis, Instance[], Instance> callable)
        where TThis : Instance
    {
        DefineMethod(name, signature, (thisInstance, arguments) => callable((TThis)thisInstance, arguments));
    }

    protected void DefineOperator(UnaryOperator unaryOperator, InstanceType returnType, InstanceUnaryOperator.CallableDelegate callable)
    {
        AssertNotDefined();

        Debug.Assert(!_UnaryOperators.ContainsKey(unaryOperator), $"unary operator {unaryOperator.GetDescription()} is already defined in type {this}");

        _UnaryOperators[unaryOperator] = new InstanceUnaryOperator(this, returnType, callable);
    }

    protected void DefineOperator<TThis>(UnaryOperator unaryOperator, InstanceType returnType, Func<TThis, Instance> callable)
        where TThis : Instance
    {
        DefineOperator(unaryOperator, returnType, thisInstance => callable((TThis)thisInstance));
    }

    protected void DefineOperator(BinaryOperator binaryOperator, InstanceType returnType, InstanceType rightType, InstanceBinaryOperator.CallableDelegate callable)
    {
        AssertNotDefined();

        // TODO: formatted operators
        Debug.Assert(!_BinaryOperators.ContainsKey((binaryOperator, rightType)), $"binary operator {this} {binaryOperator.GetDescription()} {rightType} is already defined");

        _BinaryOperators[(binaryOperator, rightType)] = new InstanceBinaryOperator(this, returnType, rightType, callable);
    }

    protected void DefineOperator<TThis, TRight>(BinaryOperator binaryOperator, InstanceType returnType, InstanceType rightType, Func<TThis, TRight, Instance> callable)
        where TThis : Instance
        where TRight : Instance
    {
        DefineOperator(binaryOperator, returnType, rightType, (thisInstance, rightInstance) => callable((TThis)thisInstance, (TRight)rightInstance));
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
