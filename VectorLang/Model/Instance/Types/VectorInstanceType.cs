namespace VectorLang.Model;

internal sealed class VectorInstanceType : InstanceType
{
    public static readonly VectorInstanceType Instance = new();

    private VectorInstanceType() : base("vector")
    {
    }

    protected override void DefineMembersInternal()
    {
        DefineField(VectorInstance.XFieldName, NumberInstanceType.Instance);
        DefineField(VectorInstance.YFieldName, NumberInstanceType.Instance);
        DefineField(VectorInstance.LengthFieldName, NumberInstanceType.Instance);

        DefineMethod("normalized", new(returnType: this), (VectorInstance thisInstance) => thisInstance.Normalized());
        DefineMethod("dot", new(returnType: NumberInstanceType.Instance), (VectorInstance thisInstance, Instance[] args) =>
        {
            return thisInstance.Dot((VectorInstance)args[0]);
        });

        DefineOperator(UnaryOperator.Plus, this, (VectorInstance thisInstance) => +thisInstance);
        DefineOperator(UnaryOperator.Minus, this, (VectorInstance thisInstance) => -thisInstance);

        DefineOperator(BinaryOperator.Plus, this, this, (VectorInstance first, VectorInstance second) => first + second);
        DefineOperator(BinaryOperator.Minus, this, this, (VectorInstance first, VectorInstance second) => first - second);

        DefineOperator(BinaryOperator.Multiply, NumberInstanceType.Instance, this, (VectorInstance first, VectorInstance second) => first * second);

        DefineOperator(BinaryOperator.Multiply, this, NumberInstanceType.Instance, (VectorInstance vector, NumberInstance number) => vector * number);
        DefineOperator(BinaryOperator.Divide, this, NumberInstanceType.Instance, (VectorInstance vector, NumberInstance number) => vector / number);
    }
}
