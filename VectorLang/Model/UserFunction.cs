using VectorLang.SyntaxTree;

namespace VectorLang.Model;

// TODO: add compilation and interpretation when these modules are finished

internal sealed class UserFunction : Function
{
    private bool _Compiled = false;

    public UserFunction(string name, CallSignature signature, ValueExpressionNode valueExpression) : base(name, signature)
    {
    }

    public void Compile()
    {
        if (_Compiled)
        {
            return;
        }

        _Compiled = true;
    }

    protected override Instance CallInternal(params Instance[] arguments)
    {
        throw new System.NotImplementedException();
    }
}
