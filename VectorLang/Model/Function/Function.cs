using System;
using System.Diagnostics;

namespace VectorLang.Model;

internal abstract class Function
{
    public string Name { get; }

    public CallSignature Signature { get; }

    public Function(string name, CallSignature signature)
    {
        Name = name;
        Signature = signature;
    }

    public Instance Call(params Instance[] arguments)
    {
        Signature.AssertArgumentsDebug(arguments);

        var returned = CallInternal(arguments);

        Debug.Assert(returned.Type.IsAssignableTo(Signature.ReturnType));

        return returned;
    }

    public Instance Call()
    {
        return Call(Array.Empty<Instance>());
    }

    protected abstract Instance CallInternal(params Instance[] arguments);
}
